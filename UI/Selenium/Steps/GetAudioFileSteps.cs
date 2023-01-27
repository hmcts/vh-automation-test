using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    ///<summary>
    /// Steps class for Get Audio File page
    ///</summary>
    public class GetAudioFileSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        DashboardSteps dashboardSteps;
        Hearing _hearing;
        GetAudioFileSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            dashboardSteps = new DashboardSteps(_scenarioContext);
        }

        [Given(@"Progress to the Get Audio File page")]
        public void GivenProgressToTheGetAudioFilePage()
        {
            dashboardSteps.SelectDashboardOption("Get audio file link");
        }

        [When(@"I search for the audio recording by case number")]
        public void WhenISearchForTheAudioRecordingByCaseNumber()
        {
            _hearing = (Hearing)_scenarioContext["Hearing"];
            ExtensionMethods.FindElementWithWait(Driver, GetAudioFilePage.CaseNumberInput, _scenarioContext).SendKeys(_hearing.Case.CaseNumber);
            ExtensionMethods.FindElementWithWait(Driver, GetAudioFilePage.SearchButton, _scenarioContext).Click();
            ExtensionMethods.WaitForElementVisible(Driver, GetAudioFilePage.GetLinkButton);
            ExtensionMethods.FindElementWithWait(Driver, GetAudioFilePage.GetLinkButton, _scenarioContext).Click();
        }

        [Then(@"the audio recording link for main hearing and for interpreter VMR can be retrieved")]
        public void ThenTheAudioRecordingLinkForMainHearingAndForInterpreterVMRCanBeRetrieved()
        {
            ExtensionMethods.WaitForElementVisible(Driver, GetAudioFilePage.CopyLinkButton);
            foreach(var element in Driver.FindElements(GetAudioFilePage.CopyLinkButton))
            {
                element.Click();
                ExtensionMethods.WaitForElementVisible(Driver, GetAudioFilePage.LinkCopiedMessage);
                ExtensionMethods.WaitForElementNotVisible(Driver, GetAudioFilePage.LinkCopiedMessage);
            }
        }
    }
}
