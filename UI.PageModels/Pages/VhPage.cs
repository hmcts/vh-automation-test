using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TestFramework;

namespace UI.PageModels.Pages;

public abstract class VhPage
{
    protected readonly By Spinner = By.Id("waitPopup");
    protected int DefaultWaitTime;
    protected IWebDriver Driver;
    protected string Locale = "en-GB";

    protected VhPage(IWebDriver driver, int defaultWaitTime)
    {
        Driver = driver;
        DefaultWaitTime = defaultWaitTime;
        if (driver is RemoteWebDriver) Locale = "en-US";
    }

    protected bool HasFormValidationError()
    {
        return Driver.FindElements(By.ClassName("govuk-error-summary")).Count > 0 ||
               Driver.FindElements(By.ClassName("govuk-error-message")).Count > 0;
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
        if (clearText) element.ClearText();

        element.SendKeys(text);
    }

    protected void WaitForApiSpinnerToDisappear(int timeout = 30)
    {
        WaitForElementToBeInvisible(Spinner, timeout);
        new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTime))
            .Until(ExpectedConditions.InvisibilityOfElementLocated(Spinner));
    }

    protected void WaitForDropdownListToPopulate(By locator)
    {
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

    protected void WaitForElementToBeClickable(By locator)
    {
        new WebDriverWait(Driver, TimeSpan.FromSeconds(DefaultWaitTime))
            .Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    protected void ClickElement(By locator)
    {
        WaitForElementToBeClickable(locator);
        try
        {
            Driver.FindElement(locator).Click();
        }
        catch (ElementClickInterceptedException ex)
        {
            if (ex.Message.Contains("id=\"waitPopup\""))
            {
                WaitForApiSpinnerToDisappear();
                ClickElement(locator);
            }
            else
            {
                throw;
            }
        }
    }

    protected void SetCheckboxValue(By locator, bool checkedValue)
    {
        WaitForElementToBeClickable(locator);
        var checkbox = Driver.FindElement(locator);
        if (checkbox.Selected != checkedValue) checkbox.Click();
    }

    protected void SelectDropDownByText(By locator, string text)
    {
        var selectElement = new SelectElement(Driver.FindElement(locator));
        selectElement.SelectByText(text);
    }

    protected void SelectDropDownByValue(By locator, string value)
    {
        var selectElement = new SelectElement(Driver.FindElement(locator));
        selectElement.SelectByValue(value);
    }

    protected void SelectDropDownByIndex(By locator, int index)
    {
        var selectElement = new SelectElement(Driver.FindElement(locator));
        selectElement.SelectByIndex(index);
    }

    protected void WaitForElementToBeVisible(By locator, int timeOut)
    {
        new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut))
            .Until(ExpectedConditions.ElementIsVisible(locator));
    }

    protected void WaitForElementToBeClickable(By locator, int timeOut)
    {
        new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut))
            .Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    protected void WaitForElementToBeInvisible(By locator, int timeOut)
    {
        new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut))
            .Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
    }

    protected string GetLocaleDate(DateTime date)
    {
        return date.ToString(new CultureInfo(Locale).DateTimeFormat.ShortDatePattern);
    }
}