using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestFramework
{
    public class ExtensionMethods
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static IWebElement FindElementWithWait(IWebDriver webdriver, By findBy, TimeSpan? waitPeriod=null)
        {
            waitPeriod = waitPeriod == null ? TimeSpan.FromSeconds(20) : waitPeriod;
            IWebElement webelement = null;
            try
            {
                //If there is no page specific timeout specified, use default timeout
                var wait = new WebDriverWait(webdriver, waitPeriod.Value);
                wait.Until(ExpectedConditions.ElementIsVisible(findBy));

                webelement = wait.Until(d =>
                {
                    try
                    {
                        var result = d.FindElements((findBy)).FirstOrDefault(elem => elem.Displayed);
                        return result;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });
            }
            catch 
            {
                //log the exception
            }
            return webelement;
        }
        public static bool IsElementVisible(IWebDriver driver, By by)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(0));
                wait.Until(ExpectedConditions.ElementIsVisible(by));
                return driver.FindElement(by).Displayed && driver.FindElement(by).Enabled;
            }
            catch
            {
                return false;
            }
        }
        public static IWebElement WaitForDropDownListItems(IWebDriver driver, By by)
        {
            bool hasItems = false;
            var el = driver.FindElement(by);
            while (!hasItems)
            {
                var ddl = new SelectElement(el);
                if(ddl != null && ddl.Options?.Count > 0)
                {
                    return el;
                }
            }

            return el;
        }

        public static SelectElement FindSelectElementWhenPopulated(IWebDriver driver, By by, int delayInSeconds, string optionText)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(delayInSeconds));
            return wait.Until<SelectElement>(drv =>
            {
                SelectElement element = new SelectElement(drv.FindElement(by));
                if (element.SelectedOption.ToString().Contains(optionText))
                {
                    return element;
                }

                return null;
            }
            );
        }

        public static SelectElement GetSelectElementWithText(IWebDriver webdriver, By selectFind, string option)
        {
            var element = ExtensionMethods.FindElementWithWait(webdriver,selectFind);
            var select = new SelectElement(element);
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(20));
            try
            {
                wait.Until(d => select.Options.Any(s => option.Equals(s.Text, StringComparison.InvariantCultureIgnoreCase)));
            }
            catch(Exception e)
            {
                logger.Error($"Option '{option}' is not available in the drop down DROPDOWNNAME on page PAGENAME");
                throw;
            }

            return select;
        }
        public static IWebElement MoveToElement(IWebDriver driver, By locator)
        {
            IWebElement el = null;
            try
            {
                Actions action = new Actions(driver);
                el = driver.FindElement(locator);
                action.MoveToElement(driver.FindElement(locator));
                action.Perform();
                return el;
            }
            catch
            {
                return el;
            }
        }

        public static IWebElement FindElementEnabledWithWait(IWebDriver webdriver, By findBy, int? waitTimeInSec = null)
        {
            int count = 0;
            bool isVisible = false;
            while (!isVisible)
            {
                try
                {
                    webdriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                    if (!webdriver.FindElement(findBy).Enabled  && count < waitTimeInSec)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        webdriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                        return webdriver.FindElement(findBy);
                    }
                }
                catch
                {
                    if(count > waitTimeInSec)
                    {
                       return null;
                    }
                }
                count++;
            }
            webdriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            return null;
        }
    }
}
