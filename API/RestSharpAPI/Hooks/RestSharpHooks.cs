using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using NUnit.Framework;
using RestSharp;
using System;
using System.IO;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

using Utilities;

namespace RestSharpApi.Hooks
{
    [Binding]
    public  class RestSharpHooks 
    {
        public static RestClient _restClient;
        public static string ProjectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        //public static string PathReport = ProjectPath + "\\TestResults\\Report\\ExtentReport.html";
        private static ExtentReports _extent;
        private static ExtentTest _feature;
        private static ExtentTest _scenario;
        public static Logger _logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        public static EnvironmentConfigSettings config;
        public IConfiguration Configuration { get; }

        public RestSharpHooks()
        {
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            try
            {
                config = TestConfigHelper.GetApplicationConfiguration();
                string PathReport = Directory.CreateDirectory(ProjectPath + Path.Combine("\\TestResults\\Report")).FullName;
                var reporter = new ExtentHtmlReporter(PathReport);
                _extent = new ExtentReports();
                _extent.AttachReporter(reporter);
                _logger.Info("Automation Test Execution Commenced");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error has occured before Automation Test Execution ");
            }
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext, ISpecFlowOutputHelper outputHelper)
        {
            var featureTitle = featureContext.FeatureInfo.Title;
            _feature = _extent.CreateTest<Feature>(featureTitle);

            _logger.Info($"Starting feature '{featureTitle}'");
        }

        [BeforeScenario("api")]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            var scenarioTitle = scenarioContext.ScenarioInfo.Title;
            _logger.Info($"Starting scenario '{scenarioTitle}'");
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            _scenario.AssignCategory(scenarioContext.ScenarioInfo.Tags);
        }
        [BeforeStep]
        public static void BeforeStep(ScenarioContext scenarioContext)
        {
            var stepTitle = scenarioContext.StepContext.StepInfo.Text;
            _logger.Info($"Starting step '{stepTitle}'");
        }

        [AfterStep]
        public static void AfterStep(ScenarioContext scenarioContext)
        {
            var stepTitle = scenarioContext.StepContext.StepInfo.Text;
            _logger.Info($"ending step '{stepTitle}'");
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            var scenarioTitle = scenarioContext.ScenarioInfo.Title;
            _logger.Info($"Ending scenario '{scenarioTitle}'");
        }

        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext, ISpecFlowOutputHelper outputHelper)
        {
            var featureTitle = featureContext.FeatureInfo.Title;
            _logger.Info($"Ending feature '{featureTitle}'");
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _logger.Info("Automation Test Execution Ended");
            LogManager.Shutdown();
        }

    }
}