using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumSpecFlow.Utilities;
using System;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using TestFramework;
using UISelenium.Pages;
using OpenQA.Selenium.Interactions;
using UI.Utilities;
using UI.Model;

namespace UI.Steps
{
    [Binding]
    public class SelectYourHearingListSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        private HearingList _hearingList;

        public SelectYourHearingListSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I choose from hearing lists")]
        public void GivenIChooseFromHearingLists(Table table)
        {
            _scenarioContext.UpdatePageName("View hearing venue list");
            foreach (var row in table.Rows)
            {
                SelectVenue(row[0]);
            }
        }

        [When(@"I click on view hearings")]
        public void WhenIClickOnViewHearings()
        {
            ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.ViewHearings, _scenarioContext).Click();
            _hearingList = new HearingList();
            _scenarioContext.Add("HearingList", _hearingList);
        }

      public void SelectVenue(string venueId)
      {
            ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.HearingList, _scenarioContext).SendKeys(venueId);
            ExtensionMethods.FindElementWithWait(Driver, SelectYourHearingListPage.HearingCheckBox, _scenarioContext).Click();
      }
    }
}

