using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumSpecFlow.Utilities;
using TechTalk.SpecFlow;

namespace UI.Steps.CommonActions
{
    [Binding]
    public class CommonPageActions
    {
        IWebDriver Driver;
        private ScenarioContext _scenarioContext;
        public CommonPageActions(ScenarioContext scenarioContext)
        {
            _scenarioContext=scenarioContext;
        }
        public CommonPageActions(IWebDriver _Driver)
        {
            Driver = _Driver;
        }
        public bool NavigateToPage(string targetUrl, string redirectUrl = null)
        {
            
            if (!Driver.Url.Contains(targetUrl))
            {
                Driver.Navigate().GoToUrl(targetUrl);
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return Driver.Url.Contains(redirectUrl);
            }
            else
            {
                return Driver.Url.Contains(targetUrl);
            }
        }

        public bool VerifyPageTitle(string title)
        {
            return Driver.Url.Contains(title);
        }

        public void MouseMoveToElement(By ele)
        {
            Actions action = new Actions(Driver);
            action.MoveToElement(Driver.FindElement(ele)).Perform();
        } 
    }
}
