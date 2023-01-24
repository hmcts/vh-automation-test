using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Select Your Hearing List
    ///</summary>
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

