using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Assign a Judge to a Hearing page
    ///</summary>
    public class HearingAssignJudgeSteps : ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;

        public HearingAssignJudgeSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _hearing = (Hearing)_scenarioContext["Hearing"];
        }

        [Given(@"I want to Assign a Judge with courtroom details")]
        public void GivenIWantToAssignAJudgeWithCourtroomDetails(Table table)
        {
            _scenarioContext.UpdatePageName("Assign a judge or courtroom account");
            _hearing = CreateHearingModel(table);
            EnterJudgeDetails(_hearing.Judge);
        }

        private Hearing CreateHearingModel(Table table)
        {
            var tableRow = table.Rows[0];
            _hearing.Judge.Email = tableRow["Judge or Courtroom Account"];
            _scenarioContext["Hearing"] = _hearing;
            return _hearing;
        }

        private void EnterJudgeDetails(Judge judge)
        {
            ExtensionMethods.WaitForElementVisible(Driver, HearingAssignJudgePage.JudgeEmail);
            ExtensionMethods.FindElementWithWait(Driver,HearingAssignJudgePage.JudgeEmail).SendKeys(judge.Email);
            ExtensionMethods.WaitForElementVisible(Driver, HearingAssignJudgePage.SearchResults);
            ExtensionMethods.FindElementWithWait(Driver, HearingAssignJudgePage.SearchResults).Click();
            Driver.FindElement(HearingAssignJudgePage.NextButton).Click();
            var participant = new Participant();
            participant.Party.Name = "Judge";
            participant.Role.Name = "Judge";
            participant.Id = judge.Email;
            _hearing.Participant.Add(participant);
            _scenarioContext["Hearing"] = _hearing;
        }
    }
}
