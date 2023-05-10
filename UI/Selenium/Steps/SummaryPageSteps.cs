using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Support.UI;
using TestFramework;
using SeleniumExtras.WaitHelpers;
using FluentAssertions;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Booking summary page
    ///</summary>
    public class SummaryPageSteps : ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        public SummaryPageSteps(ScenarioContext scenarioContext)
            :base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I book the hearing")]
        public void GivenIBookTheHearing()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(90);

            // _scenarioContext.UpdatePageName("Hearing summary");
            // ExtensionMethods.FindElementWithWait(Driver, SummaryPage.BookButton, _scenarioContext, timeout).Click();
            // ExtensionMethods.WaitForElementNotVisible(Driver, SummaryPage.DotLoader, timeout);
            // if (ExtensionMethods.IsElementExists(Driver, SummaryPage.TryAgainButton, _scenarioContext))
            // {
            //     ExtensionMethods.FindElementWithWait(Driver, SummaryPage.TryAgainButton, _scenarioContext, timeout).Click();
            //     wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SummaryPage.DotLoader));
            // }

            
            _scenarioContext.UpdatePageName("Hearing summary");
            ExtensionMethods.FindElementWithWait(Driver, SummaryPage.BookButton, _scenarioContext).Click();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.OneMinuteElementWait)));
            ExtensionMethods.WaitForElementNotVisible(Driver, SummaryPage.DotLoader, 90);
            if (ExtensionMethods.IsElementExists(Driver, SummaryPage.TryAgainButton, _scenarioContext))
            {
                ExtensionMethods.FindElementWithWait(Driver, SummaryPage.TryAgainButton, _scenarioContext).Click();
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SummaryPage.DotLoader));
            }
        }

        [Then(@"A hearing should be created")]
        public void ThenAHearingShouldBeCreated()
        {
            _scenarioContext.UpdatePageName("Hearing booking confirmation");
            var successTitle = ExtensionMethods.FindElementWithWait(Driver, SummaryPage.SuccessTitle, _scenarioContext);
            successTitle.Text.Should().Contain("Your hearing booking was successful");
            //ExtensionMethods.FindElementWithWait(Driver, BookingConfirmationPage.ViewBookingLink, _scenarioContext).Click();
            //ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.ConfirmBookingButton, _scenarioContext);
            //ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.ConfirmBookingButton, _scenarioContext).Click();
            //WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.OneMinuteElementWait)));
            //wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SummaryPage.DotLoader));
            // ExtensionMethods.FindElementWithWait(Driver, BookingDetailsPage.BookingConfirmedStatus, _scenarioContext);
        }
    }
}
