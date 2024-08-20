using System.Diagnostics;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Selenium.Axe;
using SeleniumExtras.WaitHelpers;
using UI.Common.Configuration;
using UI.Common.CustomExceptions;

namespace UI.PageModels.Pages;

public abstract class VhPage
{
    public const string VHTestNameKey = "VHTestName";
    protected readonly By Spinner = By.Id("waitPopup");
    protected int DefaultWaitTime;
    protected IWebDriver Driver;
    protected bool AccessibilityCheck;
    protected string AccessibilityReportFilePath;
    protected string AccessibilityHtmlReportFilePath;
    protected static readonly string GbLocale = "en-GB";
    protected string Locale = GbLocale;
    protected bool IsLoginPage => Driver.Url.Contains("login");
    protected bool IgnoreAccessibilityForPage = false;
    protected bool UseAltLocator;

    protected VhPage(IWebDriver driver, int defaultWaitTime, bool useAltLocator, bool ignoreAccessibilityForPage = false)
    {
        var config = ConfigRootBuilder.EnvConfigInstance();
        Driver = driver;
        DefaultWaitTime = defaultWaitTime;
        IgnoreAccessibilityForPage = ignoreAccessibilityForPage;
        AccessibilityCheck = config.EnableAccessibilityCheck;
        AccessibilityReportFilePath = config.AccessibilityReportFilePath;
        AccessibilityHtmlReportFilePath = config.AccessibilityHtmlReportFilePath;
        UseAltLocator = useAltLocator;
        if (driver is RemoteWebDriver) Locale = "en-US";
        CheckAccessibility();
    }

    private void CheckAccessibility()
    {
        ConfirmPageHasLoaded();
        if(!AccessibilityCheck || IsLoginPage || IgnoreAccessibilityForPage) return;
        var axeBuilder = new AxeBuilder(Driver);
        axeBuilder.WithOutputFile(AccessibilityReportFilePath);
        var axeResult = axeBuilder.Analyze();
        var htmlFilePath = AccessibilityHtmlReportFilePath;
        var stagingDir = Environment.GetEnvironmentVariable("BUILD_ARTIFACTSTAGINGDIRECTORY");
        if (!string.IsNullOrEmpty(stagingDir))
        {
            var testName = Environment.GetEnvironmentVariable(VHTestNameKey);
            htmlFilePath = Path.Join(stagingDir, $"{testName}_AccessibilityReport.html");
        }
        Driver.CreateAxeHtmlReport(axeResult, htmlFilePath, ReportTypes.Violations);
        if (Array.Exists(axeResult.Violations, x => x.Impact != "minor"))
            throw new AccessibilityException(GetType().Name, axeResult.Violations
                    .Where(x => x.Impact != "minor")
                    .Select(x => new AccessibilityException(x.Description)));
        
    }
    
    protected virtual void ConfirmPageHasLoaded()
    {
        // Actions needed to wait for page to load
        // Implemented by child classes
    }
    
    /// <summary>
    /// Check if the page has any validation errors (i.e. is the govuk-error-summary class present)
    /// </summary>
    /// <returns></returns>
    protected bool HasFormValidationError()
    {
        return Driver.FindElements(By.ClassName("govuk-error-summary")).Count > 0 ||
               Driver.FindElements(By.ClassName("govuk-error-message")).Count > 0;
    }

    /// <summary>
    /// Get all error messages on the page with the class govuk-error-message
    /// </summary>
    /// <returns></returns>
    protected string GetValidationErrors()
    {
        var errors = Driver.FindElements(By.ClassName("govuk-error-message")).Select(x => x.Text);
        return string.Join("; ", errors);
    }

    protected string GetText(By locator)
    {
        WaitForElementToBeVisible(locator);
        var element = new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTime)).Until(drv =>
            drv.FindElement(locator));
        return element.Text;
    }


    protected bool IsElementVisible(By locator)
    {
        try
        {
            var element = Driver.FindElement(locator);
            return element.Displayed;
        }
        catch (Exception)
        {
            return false;
        }
    }

    protected void EnterText(By locator, string text, bool clearText = true)
    {
        WaitForElementToBeVisible(locator);
        var element = new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTime)).Until(drv =>
            drv.FindElement(locator));
        if (clearText) element.Clear();

        element.SendKeys(text);
    }

    protected void WaitForApiSpinnerToDisappear(int timeout = 30)
    {
        try
        {
            WaitForElementToBeInvisible(Spinner, timeout);
            new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTime))
                .Until(ExpectedConditions.InvisibilityOfElementLocated(Spinner));
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Couldn't find spinner");
        }
    }

    protected void WaitForDropdownListToPopulate(By locator, int waitTime = 2000)
    {
        Thread.Sleep(waitTime);
        new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTime))
            .Until<SelectElement?>(drv =>
                {
                    var element = new SelectElement(drv.FindElement(locator));
                    return element.Options.Count >= 2 ? element : null;
                }
            );
    }

    protected void WaitForElementToBeVisible(By locator)
    {
        new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTime))
            .Until(ExpectedConditions.ElementIsVisible(locator));
    }
    
    protected void WaitForElementToBeVisible(By locator, int timeOut)
    {
        new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut))
            .Until(ExpectedConditions.ElementIsVisible(locator));
    }

    protected void WaitForElementToBeClickable(By locator)
    {
        new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTime))
            .Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    protected void WaitForElementToBeClickable(By locator, int timeOut, bool withRefresh = false)     
    {
        try
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut))
                .Until(ExpectedConditions.ElementToBeClickable(locator));
        }
        catch (WebDriverTimeoutException)
        {
            if (!withRefresh) throw;
            Driver.Navigate().Refresh();
            WaitForElementToBeClickable(locator, timeOut, withRefresh: false);
        }
    }
    
    protected void ClickElement(By locator, bool waitToBeClickable = true)
    {
        if (waitToBeClickable)
        {
            WaitForElementToBeClickable(locator);
        }
        try
        {
            Driver.FindElement(locator).Click();
        }
        catch (ElementClickInterceptedException ex)
        {
            if (ex.Message.Contains("waitPopup"))
            {
                WaitForApiSpinnerToDisappear();
                ClickElement(locator);
            }
            else if (ex.Message.Contains("<vh-toast"))
            {
                CloseAllToasts();
                ClickElement(locator);
            }
            else
            {
                throw;
            }
        }
    }

    private void CloseAllToasts()
    {
        Driver.FindElements(By.XPath($"(//*[contains(@id,'notification-toastr-')])")).ToList()
            .ForEach(x => x.Click());
    }

    protected void SetCheckboxValue(By locator, bool checkedValue)
    {
        WaitForElementToBeClickable(locator);
        var checkbox = Driver.FindElement(locator);
        if (checkbox.Selected != checkedValue) checkbox.Click();
    }

    protected void SelectDropDownByText(By locator, string text)
    {
        WaitForElementToBeVisible(locator, DefaultWaitTime);
        WaitForElementToBeClickable(locator);
        var selectElement = new SelectElement(Driver.FindElement(locator));
        selectElement.SelectByText(text);
    }

    protected void SelectDropDownByValue(By locator, string value)
    {
        WaitForElementToBeClickable(locator);
        var selectElement = new SelectElement(Driver.FindElement(locator));
        selectElement.SelectByValue(value);
    }

    protected void SelectDropDownByIndex(By locator, int index)
    {
        WaitForElementToBeClickable(locator);
        var selectElement = new SelectElement(Driver.FindElement(locator));
        selectElement.SelectByIndex(index);
    }


    protected void WaitForElementToBeInvisible(By locator, int timeOut)
    {
        new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut))
            .Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
    }

    protected string GetLocaleDate(DateTime date)
    {
        return date.ToString(Locale == GbLocale ? "dd/MM/yyyy" : "MM/dd/yyyy");
    }

    protected string GetLocaleTime(TimeOnly time)
    {
        return time.ToString(Locale == GbLocale ? "HHmm" : "hhmmtt");
    }

    public static void MilliTimeOut(double timeOut)
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(timeOut));
    }
    
    public bool WaitForElementVisible(IWebDriver driver, By by)
    {
        var timerCheck = TimeSpan.FromSeconds(2);
        var stopWatch = Stopwatch.StartNew();
        do
        {
            if (driver.FindElement(by).Displayed)
            {
                WaitForElementToBeClickable(by);
            }
            MilliTimeOut(200);
        } while (stopWatch.Elapsed < timerCheck);
        return driver.FindElement(by).Displayed;
    }
    
    public IWebElement WaitForSingleCondition(IWebDriver driver, By by)
    {
        driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(DefaultWaitTime);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DefaultWaitTime))
        {
            PollingInterval = TimeSpan.FromMilliseconds(500),
            Message = $"Element {by} not found"
        };
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
        return wait.Until(ElementDisplayed(by));
    }

    private static Func<IWebDriver, IWebElement> ElementDisplayed(By element)
    {
        return ((x)=>
        {
            return x.FindElement(element);
        });
    }

    /// <summary>
    /// Returns the element if it exists, otherwise returns null 
    /// </summary>
    /// <param name="by"></param>
    /// <returns></returns>
    protected IWebElement? FindElement(By by)
    {
        IWebElement? element = null;
        try
        {
            element = Driver.FindElement(by);
        }
        catch (NoSuchElementException)
        {
        }

        return element;
    }
}