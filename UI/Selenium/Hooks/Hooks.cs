using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using NLog;
using NLog.Web;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UI.Utilities;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(3)]

namespace UI.Hooks
{
    ///<summary>
    /// Class to define the code to execute at certain events during test execution
    /// Saves images after each step
    /// Closes any orphaned browser instances at the end of a Scenario and final Test Run
    ///</summary>
    [Binding]
    public class Hooks 
    {
        private static readonly Logger Logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        private static string _projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private static EnvironmentConfigSettings _config;
        private static string _pathReport ;
        private static string _imagesPath;
        private static ExtentTest _feature;
        private static ExtentTest _scenario;
        private static ExtentReports _extent;
        
        private static string _featureTitle;
        private static int _imageNumber;
        private static string _scenarioTitle;
        private static DateTime _testStartTime;
        private static string _browserName;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            try
            {
                _config = TestConfigHelper.GetApplicationConfiguration();
                _projectPath = _config.TestResultsDirectory.Equals("default")
                    ? Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
                    : _config.TestResultsDirectory;
                SetupTestReporter();
                Logger.Info("Automation Test Execution Commenced");
                _testStartTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error has occured before Automation Test Execution ");
                throw;
            }
        }

        private static void SetupTestReporter()
        {
            var logFilePath = Util.GetLogFileName("logfile");
            var logFileName = Path.GetFileNameWithoutExtension(logFilePath);
            var folderName = logFileName.Replace(":",".");
            _imagesPath=Path.Combine(_config.ImageLocation, folderName);
            Directory.CreateDirectory(_projectPath + _imagesPath);
            _pathReport= Path.Combine(_projectPath+_config.ReportLocation, folderName, "ExtentReport.html");
            var reporter = new ExtentHtmlReporter(_pathReport);
            _extent = new ExtentReports();
            _extent.AttachReporter(reporter);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _featureTitle = featureContext.FeatureInfo.Title;
            _feature = _extent.CreateTest<Feature>(_featureTitle);
            Logger.Info($"Starting feature '{_featureTitle}'");
            featureContext.Add("AccessibilityBaseUrl", "");
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _scenarioTitle = scenarioContext.ScenarioInfo.Title;
            Logger.Info($"Starting scenario '{_scenarioTitle}'");
            _imageNumber=0;
        }
        
        [BeforeScenario("web")]
        public void BeforeScenarioWeb(ScenarioContext scenarioContext,FeatureContext featureContext)
        {
            var tags = new List<string>(scenarioContext.ScenarioInfo.Tags.Concat(featureContext.FeatureInfo.Tags));
            var title = scenarioContext.ScenarioInfo.Title;
            _scenarioTitle = scenarioContext.ScenarioInfo.Title;
            _scenario = _feature.CreateNode<Scenario>(title);
            _scenario.AssignCategory(tags.ToArray());
            scenarioContext.Add("ProcessIds", new List<int>());
            _browserName = _config.BrowserType.ToString();
            IWebDriver driver;
            if (RunOnSauceLabs(tags))
            {
                var sauceOptions = new SauceLabsOptions
                {
                    Name = title
                };
                driver= new DriverFactory().InitializeSauceDriver(sauceOptions,_config.SauceLabsConfiguration);
            }
            else
            {
                driver= new DriverFactory().InitializeDriver(_config.BrowserType);
                ((List<int>)scenarioContext["ProcessIds"]).AddRange(DriverFactory.ProcessIds);
            }
            scenarioContext.Add("driver", driver);
            scenarioContext.Add("config", _config);
            scenarioContext.Add("feature", _featureTitle);
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            _scenario.AssignCategory(scenarioContext.ScenarioInfo.Tags);
            scenarioContext.Add("drivers", new Dictionary<string, IWebDriver>());
            scenarioContext.Add("AccessibilityBaseUrl", featureContext["AccessibilityBaseUrl"]);
        }

        [BeforeScenario("api")]
        public void BeforeScenarioApi(ScenarioContext scenarioContext)
        {
            _scenarioTitle = scenarioContext.ScenarioInfo.Title;
            Logger.Info($"Starting scenario '{_scenarioTitle}'");
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            _scenario.AssignCategory(scenarioContext.ScenarioInfo.Tags);
        }

        [BeforeScenario("soap")]
        public void BeforeScenarioSoapApi(ScenarioContext scenarioContext)
        {
            _scenarioTitle = scenarioContext.ScenarioInfo.Title;
            Logger.Info($"Starting scenario '{_scenarioTitle}'");
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            _scenario.AssignCategory(scenarioContext.ScenarioInfo.Tags);
        }

        [BeforeStep]
        public static void BeforeStep(ScenarioContext scenarioContext)
        {
            var stepTitle = scenarioContext.StepContext.StepInfo.Text;
            Logger.Info($"Starting step '{stepTitle}'");
        }

        [AfterStep]
        public static void AfterStep(ScenarioContext scenarioContext)
        {
            var stepTitle = scenarioContext.StepContext.StepInfo.Text;
            Logger.Info($"ending step '{stepTitle}'");
        }

        [AfterStep("web")]
        public static void InsertReportingStepsWeb(ScenarioContext scenarioContext)
        {
            var driver = (IWebDriver) scenarioContext["driver"];
            Debug.Assert(driver != null);
            var imageNumberStr = (++_imageNumber).ToString("D4");
            var imageFileName = $"{_scenarioTitle.Replace(" ", "_")}{imageNumberStr}";
            var screenshotFilePath = Path.Combine(_projectPath + _imagesPath, $"{imageFileName}.png");
            var mediaModel = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotFilePath).Build();

            var stepFailed = scenarioContext.TestError != null;
            if (stepFailed)
            {
                Logger.Warn("Step failed, executing post step failed actions");
                Debug.Assert(scenarioContext != null);
                Debug.Assert(driver != null);
                Debug.Assert(screenshotFilePath != null);
                Debug.Assert(mediaModel != null);
                ExecutePostStepFailedActions(scenarioContext, driver, screenshotFilePath, mediaModel);
            }

            if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.StepDefinitionPending)
            {
                Logger.Debug("Step definition pending, executing post step definition pending actions");
                Debug.Assert(scenarioContext != null);
                Debug.Assert(mediaModel != null);
                ExecutePostStepDefinitionPendingActions(scenarioContext, mediaModel);
            }

            if (!stepFailed)
            {
                Logger.Debug("Step succeeded, executing post step success actions");
                Debug.Assert(scenarioContext != null);
                Debug.Assert(driver != null);
                Debug.Assert(screenshotFilePath != null);
                Debug.Assert(mediaModel != null);
                ExecutePostStepSuccessActions(scenarioContext, screenshotFilePath, mediaModel, driver);
            }

            if (scenarioContext.StepContext.StepInfo.Text.Equals("I log off"))
            {
                driver.Close();
                driver.Quit();
                scenarioContext.Remove("driver");
            }
        }

        private static void ExecutePostStepSuccessActions(ScenarioContext scenarioContext, string screenshotFilePath,
            MediaEntityModelProvider mediaModel, IWebDriver driver)
        {
            driver.TakeScreenshot().SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);
            Logger.Info($"Screenshot has been saved to {screenshotFilePath}");
            //For Extent report
            switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case TechTalk.SpecFlow.Bindings.StepDefinitionType.Given:
                    _scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Pass(string.Empty, mediaModel);
                    break;
                case TechTalk.SpecFlow.Bindings.StepDefinitionType.When:
                    _scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Pass(string.Empty, mediaModel);
                    break;
                case TechTalk.SpecFlow.Bindings.StepDefinitionType.Then:
                    _scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Pass(string.Empty, mediaModel);
                    break;
            }
        }

        private static void ExecutePostStepDefinitionPendingActions(ScenarioContext scenarioContext,
            MediaEntityModelProvider mediaModel)
        {
            switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case TechTalk.SpecFlow.Bindings.StepDefinitionType.Given:
                    _scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text)
                        .Skip("Step Definition Pending", mediaModel);
                    break;

                case TechTalk.SpecFlow.Bindings.StepDefinitionType.When:
                    _scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text)
                        .Skip("Step Definition Pending", mediaModel);
                    break;

                case TechTalk.SpecFlow.Bindings.StepDefinitionType.Then:
                    _scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text)
                        .Skip("Step Definition Pending", mediaModel);
                    break;
            }
        }

        private static void ExecutePostStepFailedActions(ScenarioContext scenarioContext, IWebDriver driver,
            string screenshotFilePath, MediaEntityModelProvider mediaModel)
        {
            var stepTitle = scenarioContext.StepContext.StepInfo.Text;
            Logger.Error(scenarioContext.TestError, $"Exception occured while executing step:'{stepTitle}'");
            var infoTextBuilder = new StringBuilder();
            var actionName = scenarioContext.GetActionName();
            if (!string.IsNullOrWhiteSpace(actionName))
            {
                infoTextBuilder.Append($"Action '{actionName}'");
            }

            var elementName = scenarioContext.GetElementName();
            if (!string.IsNullOrWhiteSpace(elementName))
            {
                infoTextBuilder.Append($",erred on Element '{elementName}'");
            }

            var pageName = scenarioContext.GetPageName();
            if (!string.IsNullOrWhiteSpace(pageName))
            {
                infoTextBuilder.Append($",on Page '{pageName}'");
            }

            var userName = scenarioContext.GetUserName();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                infoTextBuilder.Append($",for User '{userName}");
            }

            var infoText = infoTextBuilder.ToString();
            if (!string.IsNullOrEmpty(infoText))
            {
                Logger.Info(infoText);
            }

            driver.TakeScreenshot().SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);
            Logger.Info($"Screenshot has been saved to {screenshotFilePath}");

            switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case TechTalk.SpecFlow.Bindings.StepDefinitionType.Given:
                    _scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text)
                        .Fail(scenarioContext.TestError.Message, mediaModel);
                    break;

                case TechTalk.SpecFlow.Bindings.StepDefinitionType.When:
                    _scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text)
                        .Fail(scenarioContext.TestError.Message, mediaModel);
                    break;

                case TechTalk.SpecFlow.Bindings.StepDefinitionType.Then:
                    _scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text)
                        .Fail(scenarioContext.TestError.Message, mediaModel);
                    break;
            }

            var error = scenarioContext.TestError;
            var sb = new StringBuilder(error.Message);
            while (error.InnerException != null)
            {
                error = error.InnerException;
                sb.AppendLine(error.Message);
            }

            var failureMessage = sb.ToString();
            Assert.Fail(failureMessage);
        }

        [AfterStep("api", "soap")]
        public static void InsertReportingStepsApi(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null)
            {
                switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
                {
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.Given:
                        _scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                        break;
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.When:
                        _scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                        break;
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.Then:
                        _scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                        break;
                }
            }
            if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.StepDefinitionPending)
            {
                switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
                {
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.Given:
                        _scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                        break;
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.When:
                        _scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                        break;
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.Then:
                        _scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                        break;
                }
            }
            if (scenarioContext.TestError == null)
            {
                switch (scenarioContext.StepContext.StepInfo.StepDefinitionType)
                {
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.Given:
                        _scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Pass(string.Empty);
                        break;
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.When:
                        _scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Pass(string.Empty);
                        break;
                    case TechTalk.SpecFlow.Bindings.StepDefinitionType.Then:
                        _scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Pass(string.Empty);
                        break;
                }
            }
        }

        [AfterScenario("web")]
        public void AfterScenarioWeb(ScenarioContext scenarioContext,FeatureContext featureContext)
        {
            var driver = (IWebDriver)scenarioContext["driver"];
            LogTestResultOnSauceLabs(_config.RunOnSaucelabs, driver);
            featureContext["AccessibilityBaseUrl"] = scenarioContext["AccessibilityBaseUrl"];
            StopAllDrivers(scenarioContext);
            _extent.Flush();
            Logger.Info("Flush Extent Report Instance");
            GC.SuppressFinalize(this);
        }

        [AfterScenario("api", "soap")]
        public void AfterScenarioApi()
        {
            _extent.Flush();
            GC.SuppressFinalize(this);
        }

        [AfterTestRun]
        public static void AfterTestRun(ScenarioContext scenarioContext)
        {
            // var driver = (IWebDriver)scenarioContext["driver"];
            // LogTestResultOnSauceLabs(_config.RunOnSaucelabs, driver);
            KillAllBrowserInstances(_browserName);
            Logger.Info("Automation Test Execution Ended");
            LogManager.Shutdown();
        }

        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext)
        {
            var featureTitle = featureContext.FeatureInfo.Title;
            Logger.Info($"Ending feature '{featureTitle}'");
        }

        private static bool RunOnSauceLabs(List<string> tags)
        {
            return _config.RunOnSaucelabs && tags.Any(s => s.Contains("DeviceTest"));
        }

        private static void KillAllBrowserInstances(string processName)
        {
            var processes = Process.GetProcesses().Where(p => p.ProcessName.ToLowerInvariant().Contains(processName.ToLowerInvariant()));
            foreach (var process in processes)
            {
                try
                {
                    if (process.StartTime > _testStartTime)
                    {
                        process.Kill(true);
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        private static void KillAllBrowserInstances(ScenarioContext context)
        {
            var pcs = (List<int>)context["ProcessIds"];
            foreach (var process in pcs)
            {
                try
                {
                    Process.GetProcessById(process).Kill();
                }
                catch
                {
                    Logger.Info("There was an issue ending a browser instance");
                }
            }
        }

        private static void StopAllDrivers(Object obj)
        {
            try
            {
                var objectType = obj.GetType().Name;
                if (objectType == "ScenarioContext")
                {
                    var context = (ScenarioContext)obj;
                    var drivers = (Dictionary<string, IWebDriver>)context["drivers"];
                    foreach (var driver in drivers)
                    {
                        _browserName = $@"{((WebDriver)driver.Value).Capabilities["browserName"]}";
                        driver.Value?.Close();
                        driver.Value?.Quit();
                        Logger.Info($"Driver has been closed");
                    }
                    context.Remove("drivers");
                    if (context.ContainsKey("driver"))
                    {
                        var driver = (IWebDriver)context["driver"];
                        driver?.Close();
                        driver.Quit();
                        Logger.Info($"Driver has been closed");
                        context.Remove("driver");
                    }
                }
            }
            catch
            {
                KillAllBrowserInstances((ScenarioContext)obj);
            }
        }
            
        
        private static void LogTestResultOnSauceLabs(bool runningOnSauceLabs, IWebDriver driver)
        {
            if (!runningOnSauceLabs) return;
            SauceLabsResult.LogPassed(TestContext.CurrentContext.Result.Outcome == ResultState.Success, driver);
        }
    }
}