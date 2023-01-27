using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Polly;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using TechTalk.SpecFlow;
namespace TestFramework
{
    ///<summary>
    /// Class to extend IWebDriver,IWebElement for custom functionality
    ///</summary>
    public static class ExtensionMethods
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void AddOrUpdate<T>(this ScenarioContext scenarioContext,string key,T value)
        {
            if(scenarioContext.ContainsKey(key))
            {
                scenarioContext[key]=value;
            }
            else
            {
                scenarioContext.Add(key, value);
            }
        }

        public static void UpdateUserName(this ScenarioContext scenarioContext, string value)
        {
            var key = "UserName";
            scenarioContext.AddOrUpdate(key, value);
        }
        public static void UpdatePageName(this ScenarioContext scenarioContext, string value)
        {
            var key = "PageName";
            scenarioContext.AddOrUpdate(key, value);
        }
        public static void UpdateElementName(this ScenarioContext scenarioContext, string value)
        {
            var key = "ElementName";
            scenarioContext.AddOrUpdate(key, value);
        }
        public static void UpdateActionName(this ScenarioContext scenarioContext, string value)
        {
            var key = "ActionName";
            scenarioContext.AddOrUpdate(key, value);
        }

        public static string GetActionName(this ScenarioContext scenarioContext)
        {
            var key = "ActionName";
            scenarioContext.TryGetValue<string>(key,out string value);
            return value;
        }

        public static string GetUserName(this ScenarioContext scenarioContext)
        {
            var key = "UserName";
            scenarioContext.TryGetValue<string>(key, out string value);
            return value;
        }

        public static string GetPageName(this ScenarioContext scenarioContext)
        {
            var key = "PageName";
            scenarioContext.TryGetValue<string>(key, out string value);
            return value;
        }

        public static string GetElementName(this ScenarioContext scenarioContext)
        {
            var key = "ElementName";
            scenarioContext.TryGetValue<string>(key, out string value);
            return value;
        }

        public static bool IsDisplayed(this IWebDriver webdriver, By findBy, ScenarioContext scenarioContext, TimeSpan? waitPeriod = null)
        {
            var isDisplayed = false;
            var timeoutAfterPolicy = Policy.Timeout(waitPeriod.Value);
            var retryPolicy = Policy.Handle<StaleElementReferenceException>()
                              .WaitAndRetryForever(iteration => TimeSpan.FromSeconds(1));
            isDisplayed= timeoutAfterPolicy.Wrap(retryPolicy).Execute(() =>
            {
                var element = FindElementWithWait(webdriver, findBy, scenarioContext, waitPeriod);
                return element.Displayed;
            });
            return isDisplayed;
        }

        public static void RetryClick(this IWebDriver webdriver, By findBy, ScenarioContext scenarioContext, TimeSpan? waitPeriod = null)
        {
            var retryPolicy = Policy.Handle<StaleElementReferenceException>()
                                    .Or<NullReferenceException>()
                                    .Or<ElementClickInterceptedException>()
                              .WaitAndRetry(5, iteration => TimeSpan.FromSeconds(1));
            retryPolicy.Execute(() =>
            {
                var element = FindElementWithWait(webdriver, findBy, scenarioContext, waitPeriod);
                if (element == null)
                {
                    Logger.Debug($"Unable to find element {findBy}");
                }

                element.Click();
            });
        }

        public static void RetryClick(this IWebElement parentElement, By findBy)
        {
            var retryPolicy = Policy.Handle<StaleElementReferenceException>()
                                    .Or<NullReferenceException>()
                              .WaitAndRetry(5, iteration => TimeSpan.FromSeconds(1));
            retryPolicy.Execute(() =>
            {
                var element = parentElement.FindElement(findBy);
                element.Click();
            });
        }

        public static void RetryCheckboxClick(this IWebElement parentElement, By findBy)
        {
            var retryPolicy = Policy.Handle<StaleElementReferenceException>()
                                    .Or<NullReferenceException>()
                              .WaitAndRetry(5, iteration => TimeSpan.FromSeconds(1));
            retryPolicy.Execute(() =>
            {
                var element = parentElement.FindElement(findBy);
                element.Click();
                if (!element.Selected)
                {
                    throw new NullReferenceException("Checkbox not checked");
                }
            });
        }

        public static IWebElement FindElementWithWait(IWebDriver webdriver, By findBy, ScenarioContext scenarioContext,TimeSpan? waitPeriod=null)
        {
            waitPeriod = waitPeriod == null ? TimeSpan.FromSeconds(60) : waitPeriod.Value;
            IWebElement webelement = null;
            var pageName = scenarioContext?.GetPageName();
            var userName = scenarioContext?.GetUserName();
            try
            {
                //If there is no page specific timeout specified, use default timeout
                var wait = new WebDriverWait(webdriver, waitPeriod.Value);
                wait.Until(ExpectedConditions.ElementIsVisible(findBy));
                webelement = wait.Until(d =>
                {
                    try
                    {
                        var result = d.FindElements(findBy)?.FirstOrDefault(elem => elem.Displayed);
                        return result;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Cannot find element By locator:'{findBy.Criteria}' on page:'{pageName}, logged in User: {userName}");
                throw;
            }
            return webelement;
        }

        public static bool IsElementVisible(IWebDriver driver, By by, ScenarioContext scenarioContext)
        {
            try
            {
                // All we want here is a true or false where or not the element is visible, there's no need for wait.Until
                // The implicit wait (0) makes sure that that the driver wastes no time, result (true/false) is instant
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                if(driver.FindElement(by).Displayed && driver.FindElement(by).Enabled)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static bool IsElementExists(IWebDriver driver, By by, ScenarioContext scenarioContext)
        {
            var pageName = scenarioContext.GetPageName();
            var userName = scenarioContext.GetUserName();

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(0));
                wait.Until(ExpectedConditions.ElementExists(by));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Element By locator:'{by.Criteria}' doesn't exist on page:'{pageName}, logged in User: {userName}");
                return false;
            }
        }
        public static bool IsElementNotExists(IWebDriver driver, By by, ScenarioContext scenarioContext)
        {
            var pageName = scenarioContext.GetPageName();
            var userName = scenarioContext.GetUserName();

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(0));
                wait.Until(ExpectedConditions.ElementExists(by));
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Element By locator:'{by.Criteria}' exists on page:'{pageName}, logged in User: {userName}");
                return true;
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

        public static SelectElement GetSelectElementWithText(IWebDriver webdriver, By selectFind, string option,ScenarioContext scenarioContext)
        {
            var element = FindElementWithWait(webdriver,selectFind, scenarioContext);
            var select = new SelectElement(element);
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(20));
            var pageName = scenarioContext.GetPageName();
            var userName = scenarioContext.GetUserName();

            try
            {
                wait.Until(d => select.Options.Any(s => option.Equals(s.Text, StringComparison.InvariantCultureIgnoreCase)));
            }
            catch(Exception ex)
            {
                Logger.Error(ex,$"Option '{option}' is not available in the drop down locator '{selectFind.Criteria}' on page:'{pageName}, logged in User: {userName}");
                throw;
            }
            return select;
        }
        public static IWebElement MoveToElement(IWebDriver driver, By locator, ScenarioContext scenarioContext)
        {
            IWebElement el = null;
            var pageName = scenarioContext.GetPageName();
            var userName = scenarioContext.GetUserName();

            try
            {
                Actions action = new Actions(driver);
                el = driver.FindElement(locator);
                action.MoveToElement(driver.FindElement(locator));
                action.Perform();
                return el;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Cannot Move to element By locator:'{locator.Criteria}' on page:'{pageName}, logged in User: {userName}");
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
                        return webdriver.FindElement(findBy);
                    }
                }
                catch
                {
                    if (count > waitTimeInSec)
                    {
                        return null;
                    }
                }
                count++;
            }
            return null;
        }

        public static bool IsElementEnabled(IWebDriver driver, By by)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                return driver.FindElement(by).Enabled;
            }
            catch
            {
                return false;
            }
        }

        public static string GetText(IWebDriver driver, By by, ScenarioContext scenarioContext)
        {
            try
            {
                return driver.FindElement(by).Text;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Cannot Move to element By locator:'{by.Criteria}' on page:'{scenarioContext.GetPageName()}, logged in User: {scenarioContext.GetUserName()}");
            }
            return null;
        }

        public static string GetValue(IWebDriver driver, By by, ScenarioContext scenarioContext)
        {
            try
            {
                return driver.FindElement(by).GetAttribute("value");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Cannot Move to element By locator:'{by.Criteria}' on page:'{scenarioContext.GetPageName()}, logged in User: {scenarioContext.GetUserName()}");
            }
            return null;
        }

        public static void ClickAll(IWebDriver driver, By by)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            foreach (var el in driver.FindElements(by))
            {
                try
                {
                    el.Click();
                }
                catch
                {
                }
            }
        }

        public static void SendKeys(IWebDriver driver, string keys)
        {
            Actions action = new Actions(driver);
            action.SendKeys(keys);
            action.Perform();
        }

        public static void OpenNewPage(IWebDriver webdriver, String url)
        {
            try
            {
                ((IJavaScriptExecutor)webdriver).ExecuteScript("window.open();");
                webdriver = webdriver.SwitchTo().Window(webdriver.WindowHandles.Last());
                webdriver.Navigate().GoToUrl(url); 
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Cannot Move to element By locator:");
               
            }
        }
       
        public static void CloseAndOpenBrowser(IWebDriver webDriver, String url)
        {
            webDriver.FindElement(By.CssSelector("#logout-link")).Click();
            ((IJavaScriptExecutor)webDriver).ExecuteScript("window.open();");
            webDriver.Close();
            webDriver = webDriver.SwitchTo().Window(webDriver.WindowHandles.Last());
            webDriver.Navigate().GoToUrl(url);
        }

        public static bool WaitForPageLoad(IWebDriver driver, By by, ScenarioContext scenarioContext)
        {
            var pageName = scenarioContext.GetPageName();
            var userName = scenarioContext.GetUserName();

            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                wait.Until(ExpectedConditions.ElementExists(by));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Wait for Page load failed");
                return false;
            }
        }

        public static void WaitForElementNotVisible(IWebDriver driver, By by, int? timeInSec = null)
        {
            bool isVisible = true;
            timeInSec = timeInSec == null ? 20 : timeInSec.Value;
            int count = 1;
            while (isVisible)
            {
                try
                {
                    isVisible = IsElementVisible(driver, by, null);
                    if (isVisible)
                    {
                        // sleep here is only acting as a clock tick/counter really
                        // rather than as an explicit wait
                        // the advantage here is that the code continues to execute and can check for errors
                        // and immediately terminate if errors found rather than having a wait.Until() which
                        // could be be 60 sec before the terminates and thus slowing the tests down
                        System.Threading.Thread.Sleep(1000);
                    }
                    if (driver.Url.Contains("/error"))
                    {
                        string error = driver.FindElement(By.TagName("app-eeror")).Text;
                        NUnit.Framework.Assert.Fail(error);
                    }
                }
                catch
                {
                }
                if (count > timeInSec && isVisible)
                {
                    Logger.Error($"Element {by.Criteria} was not expected to be visible after {timeInSec} sec, but it was");
                    throw new Exception($"Element {by.Criteria} was not expected to be visible after {timeInSec} sec, but it was");
                }
                count++;
            }
        }

        public static void WaitForElementVisible(IWebDriver driver, By by, int? timeInSec = null)
        {
            bool isVisible = false;
            timeInSec = timeInSec == null ? 30 : timeInSec.Value;
            int count = 1;
            string error = string.Empty;
            while (!isVisible)
            {
                try
                {
                    isVisible = IsElementVisible(driver, by, null);
                    if (!isVisible)
                    {
                        // sleep here is only acting as a clock tick/counter really
                        // rather than as an explicit wait
                        // the advantage here is that the code continues to execute and can check for errors
                        // and immediately terminate if errors found rather than having a wait.Until() which
                        // could be be 60 sec before the terminates and thus slowing the tests down
                        System.Threading.Thread.Sleep(1000);
                    }
                    if (driver.Url.Contains("/error"))
                    {
                        error = driver.FindElement(By.TagName("app-error")).Text;
                        NUnit.Framework.Assert.Fail(error);
                    }
                }
                catch
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        NUnit.Framework.Assert.Fail(error);
                    }
                }
                if (count > timeInSec && !isVisible)
                {
                    Logger.Error($"Element {by.Criteria} was expected to be visible after {timeInSec} sec, but it was not");
                    throw new Exception($"Element {by.Criteria} was expected to be visible after {timeInSec} sec, but it was not");
                }
                count++;
            }
        }

        public static IWebElement FindElementWithWait(IWebDriver driver, By findBy, int? waitPeriod = null)
        {
            waitPeriod = waitPeriod == null ? 60 : waitPeriod.Value;
            var timeStart = new TimeSpan(DateTime.Now.Ticks);
            bool isVisible = IsElementVisible(driver, findBy, null);
            while (!isVisible)
            {
                if (driver.FindElement(By.TagName("body")).Text.Contains("You've been signed out of the service"))
                {
                    driver.Navigate().Back();
                }
                var timeSpent = Math.Abs((timeStart - new TimeSpan(DateTime.Now.Ticks)).TotalSeconds);
                if (timeSpent > waitPeriod)
                {
                    return null;
                }
                isVisible = IsElementVisible(driver, findBy, null);
            }
            if (isVisible)
            {
                return driver.FindElement(findBy);
            }
            return null;
        }

        public static void ClearTextByJS(this IWebDriver driver, string id, int? timeInSec = null)
        {
            var isCleared = false;
            timeInSec = timeInSec == null ? 30 : timeInSec.Value;
            var count = 1;
            while (!isCleared)
            {
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript($"document.getElementById('{id}').value=''");
                    var text = ((IJavaScriptExecutor)driver).ExecuteScript($"return document.getElementById('{id}').value") as string;
                    if (string.IsNullOrEmpty(text))
                    {
                        isCleared=true;
                    }
                }
                catch
                {
                    Logger.Error($"Exception occured while clearing text for element by Id {id}");
                }
                if (count > timeInSec && !isCleared)
                {
                    Logger.Error($"Element by Id {id} text was expected to be deleted , but it was not");
                    throw new Exception($"Element by Id {id} text was expected to be deleted , but it was not");
                }
                count++;
            }
        }

        public static void ClearText(this IWebElement element, int? timeInSec = null)
        {
            var isCleared = false;
            timeInSec = timeInSec == null ? 30 : timeInSec.Value;
            var count = 1;
            while(!isCleared)
            {
                try
                {
                    element.Clear();
                    var text = element.Text;
                    if(string.IsNullOrEmpty(text))
                    {
                        isCleared=true;
                    }
                }
                catch
                {
                    Logger.Error($"Exception occured while clearing text for element {element}");
                }
                if (count > timeInSec && !isCleared)
                {
                    Logger.Error($"Element {element} text was expected to be deleted , but it was not");
                    throw new Exception($"Element {element} text was expected to be deleted , but it was not");
                }
                count++;
            }
        }
        public static void SwitchToIframe(IWebDriver driver, By by)
        {
            driver.SwitchTo().Frame(driver.FindElement(by));
        }

        public static void WaitForTextPresent(IWebDriver driver, string text, int? timeInSec = null)
        {
            bool isPresent = false;
            timeInSec = timeInSec == null ? 30 : timeInSec.Value;
            int count = 1;
            while (!isPresent)
            {
                try
                {
                    isPresent = driver.FindElement(By.TagName("body")).Text.Contains(text);
                    if (!isPresent)
                    {
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                        wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(By.TagName("body")), text));
                    }
                }
                catch
                {

                }
                if (count > timeInSec && !isPresent)
                {
                    Logger.Error($"Text {text} was expected to be present after {timeInSec} sec, but it was not");
                    throw new Exception($"Text {text} was expected to be present after {timeInSec} sec, but it was not");
                }
                count++;
            }
        }

        public static void CheckForUnExpectedErrors(IWebDriver driver)
        {
            System.Threading.Thread.Sleep(2000); // Needed here as for some reason without it, we're getting 'your not authorised when signing in'
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            if (driver.FindElement(By.TagName("body")).Text.Contains("You've been signed out of the service")
                || driver.FindElement(By.TagName("body")).Text.Contains("403 Forbidden"))
            {
                driver.Navigate().Back();
            }
            else if (driver.Url.Contains("unauthorised"))
            {
                driver.Navigate().GoToUrl(driver.Url.Replace("unauthorised", ""));
            }
        }

        public static void AcceptAlert(IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert().Accept();
            }
            catch
            {
            }
        }

        public static void WaitFor(int timeInSec)
        {
            System.Threading.Thread.Sleep(timeInSec * 1000);
        }
    }
}
