using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using UI.Model;
using WebDriverManager.DriverConfigs.Impl;

namespace UI.Utilities
{
    /// <summary>
    /// Class to initialize new Webdriver
    /// Can create different types of browser drivers
    /// </summary>
    public class DriverFactory
    {
        private IWebDriver WebDriver { get; set; }
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static List<int> ProcessIds = new();

        public IWebDriver InitializeDriver(BrowserType browser)
        {
            IWebDriver webDriver;
            switch (browser)
            {
                case BrowserType.Firefox:
                    webDriver = BuildFireFoxDriver();
                    break;
                case BrowserType.Edge:
                    webDriver = BuildEdgeDriver();
                    break;
                case BrowserType.Safari:
                    webDriver = BuildSafariDriver();
                    break;
                case BrowserType.Chrome:
                    // var drivers = InitMultipleChromeDrivers();
                    // webDriver = drivers[0];
                    webDriver = BuildChromeDriver();
                    break;
                case BrowserType.Remote:
                default:
                    throw new Exception("RunOnSauceLabs set to false but an browser was not provided");
            }

            WebDriver = webDriver;
            WebDriver.Manage().Window.Maximize();
            return WebDriver;
        }

        private IWebDriver BuildFireFoxDriver()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
            var firefoxOptions = new FirefoxOptions();
            firefoxOptions.AddArguments("start-maximized");
            firefoxOptions.AddArgument("no-sandbox");
            firefoxOptions.AddArguments("--use-fake-ui-for-media-stream");
            firefoxOptions.AddArguments("--use-fake-device-for-media-stream");
            var webDriver = new FirefoxDriver(firefoxOptions);
            return webDriver;
            // WebDriver.Manage().Window.Maximize();
            // Logger.Info(" Firefox started in maximized mode");
            // return WebDriver
        }

        private IWebDriver BuildEdgeDriver()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
            var edgeOptions = new EdgeOptions();
            edgeOptions.AddArguments("start-maximized");
            edgeOptions.AddArgument("no-sandbox");
            edgeOptions.AddArguments("--use-fake-ui-for-media-stream");
            edgeOptions.AddArguments("--use-fake-device-for-media-stream");
            edgeOptions.AddArgument("inprivate");

            var webdriver = new EdgeDriver(edgeOptions);
            return webdriver;
            // WebDriver = new EdgeDriver(edgeoptions);
            // Logger.Info(" Edge started in maximized mode");
        }

        private IWebDriver BuildSafariDriver()
        {
            var safariOptions = new SafariOptions();
            var webDriver = new SafariDriver(safariOptions);
            return webDriver;
            // WebDriver.Manage().Window.Maximize();
            // Logger.Info(" Safari started in maximized mode");
        }

        private IWebDriver BuildChromeDriver()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            var cService = ChromeDriverService.CreateDefaultService();
            ChromeOptions chromeoptions = new ChromeOptions();
            chromeoptions.AddArguments("start-maximized");
            chromeoptions.AddArgument("no-sandbox");
            chromeoptions.AddArguments("--use-fake-ui-for-media-stream");
            chromeoptions.AddArguments("--use-fake-device-for-media-stream");
            var webDriver = new ChromeDriver(cService, chromeoptions);
            ProcessIds.Add(cService.ProcessId);
            return webDriver;
        }

        /// <summary>
        /// Unsure why only chrome creates 3 instances
        /// </summary>
        /// <returns></returns>
        private List<IWebDriver> InitMultipleChromeDrivers()
        {
            var drivers = new List<IWebDriver>();
            for (var i = 1; i < 3; i++)
            {
                try
                {
                    var webDriver = BuildChromeDriver();
                    drivers.Add(webDriver);
                }
                catch (Exception ex)
                {
                    if (i > 1 && ex.Message.Contains(
                            "The HTTP request to the remote WebDriver server for URL http://localhost"))
                    {
                        NUnit.Framework.Assert.Fail(
                            $"Chrome failed to start after {i} attempts with exception - {ex.Message}");
                    }
                }
            }

            return drivers;
        }

        public IWebDriver InitializeSauceDriver(SauceLabsOptions sauceLabsOptions, SauceLabsConfiguration config)
        {
            var sauceOptions = new Dictionary<string, object>
            {
                {"username", config.SauceUsername},
                {"accessKey", config.SauceAccessKey},
                {"name", sauceLabsOptions.Name}, {"commandTimeout", sauceLabsOptions.CommandTimeoutInSeconds},
                {"idleTimeout", sauceLabsOptions.IdleTimeoutInSeconds},
                {"maxDuration", sauceLabsOptions.MaxDurationInSeconds},
                {"seleniumVersion", sauceLabsOptions.SeleniumVersion}, {"timeZone", sauceLabsOptions.Timezone}
            };
            var remoteUrl = new Uri($"https://{config.SauceUsername}:{config.SauceAccessKey}{config.SauceUrl}");

            switch (config.PlatformName)
            {
                case "Android":
                    var options = InitAndroidAppiumOptions(sauceLabsOptions, config, sauceOptions);
                    WebDriver = new RemoteWebDriver(remoteUrl, options.ToCapabilities());
                    break;
                case "iOS":
                    var iosOptions = InitIOsAppiumOptions(sauceLabsOptions, config, sauceOptions);
                    WebDriver = new RemoteWebDriver(remoteUrl, iosOptions.ToCapabilities());
                    break;
                case "macOS 12":
                    DriverOptions driverOptions = config.BrowserName switch
                    {
                        "chrome" => new ChromeOptions(),
                        "Firefox" => new FirefoxOptions(),
                        "Edge" => new EdgeOptions(),
                        "Safari" => new SafariOptions(),
                        _ => null
                    };

                    Debug.Assert(driverOptions != null, nameof(driverOptions) + " != null");
                    driverOptions.PlatformName = config.PlatformName;
                    foreach (var (key, value) in sauceOptions)
                    {
                        driverOptions.AddAdditionalOption(key, value);
                    }

                    WebDriver = new RemoteWebDriver(remoteUrl, driverOptions);
                    break;
            }

            return WebDriver;
        }

        private static AppiumOptions InitIOsAppiumOptions(SauceLabsOptions sauceLabsOptions, SauceLabsConfiguration config,
            Dictionary<string, object> sauceOptions)
        {
            var iosOptions = new AppiumOptions
            {
                DeviceName = config.DeviceName,
                PlatformName = config.PlatformName,
                BrowserName = config.BrowserName
            };
            iosOptions.AddAdditionalAppiumOption(MobileCapabilityType.AppiumVersion, config.AppiumVersion);
            iosOptions.AddAdditionalAppiumOption(MobileCapabilityType.Orientation, config.Orientation);
            iosOptions.AddAdditionalAppiumOption("PlatformVersion", config.PlatformVersion);
            iosOptions.AddAdditionalAppiumOption("name", sauceLabsOptions.Name);
            foreach (var (key, value) in sauceOptions)
            {
                iosOptions.AddAdditionalCapability(key, value);
            }

            return iosOptions;
        }

        private static AppiumOptions InitAndroidAppiumOptions(SauceLabsOptions sauceLabsOptions, SauceLabsConfiguration config,
            Dictionary<string, object> sauceOptions)
        {
            var options = new AppiumOptions
            {
                DeviceName = config.DeviceName,
                PlatformName = config.PlatformName,
                BrowserName = config.BrowserName
            };
            options.AddAdditionalAppiumOption(MobileCapabilityType.AppiumVersion, config.AppiumVersion);
            options.AddAdditionalAppiumOption(MobileCapabilityType.Orientation, config.Orientation);
            options.AddAdditionalAppiumOption("PlatformVersion", config.PlatformVersion);
            options.AddAdditionalAppiumOption("name", sauceLabsOptions.Name);
            foreach (var (key, value) in sauceOptions)
            {
                options.AddAdditionalCapability(key, value);
            }

            return options;
        }
    }
}

