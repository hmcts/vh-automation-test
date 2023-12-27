namespace UI.PageModels.Pages.Admin.Booking;

public class HearingAssignJudgePage : VhAdminWebPage
{
    private readonly By _judgeDisplayNameFld = By.Id("judgeDisplayNameFld");
    private readonly By _ejudgeDisplayNameFld = By.Id("judiciaryDisplayNameInput");
    private readonly By _judgeEmail = By.Id("judge-email");
    private readonly By _eJudgeEmail = By.Id("judiciaryEmailInput");
    private readonly By _judgePhoneFld = By.Id("judgePhoneFld");
    private readonly By _nextButton = By.Id("nextButton");
    private readonly By _nextButtonEJudge = By.XPath("//*[@id='nextButtonToParticipants'] | //*[@id='nextButton']");
    private readonly By _searchResults = By.Id("search-results-list");
    private readonly By _saveEJudge = By.Id("confirmJudiciaryMemberBtn");
    

    public HearingAssignJudgePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
    }

    public void EnterJudgeDetails(string judgeEmail, string judgeDisplayName, string judgePhone)
    {
        EnterText(_judgeEmail, judgeEmail);
        WaitForElementToBeVisible(Spinner); // wait 2 seconds before search starts
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeVisible(_searchResults);
        ClickElement(_searchResults);
        if (!string.IsNullOrWhiteSpace(judgeDisplayName)) EnterText(_judgeDisplayNameFld, judgeDisplayName);

        if (!string.IsNullOrWhiteSpace(judgePhone)) EnterText(_judgePhoneFld, judgePhone);
    }
    
    // public ParticipantsPage GoToParticipantsPage()
    // {
    //     ClickElement(_nextButton);
    //     return new ParticipantsPage(Driver, DefaultWaitTime);
    // }
    
    // public ParticipantsPage GoToParticipantsPage()
    // {
    //     ClickElement(_nextButton);
    //     return new ParticipantsPage(Driver, DefaultWaitTime);
    // }
    //
    // public ParticipantsPage GoToParticipantsPageWithParty()
    // {
    //     WaitForElementVisible(Driver, _nextButtonEJudge);
    //     ClickElement(_nextButtonEJudge);
    //     bool useParty = true;
    //     return new ParticipantsPage(Driver, DefaultWaitTime, useParty);
    // }
    
    public void AssignPresidingJudgeDetails(string judgeEmail, string judgeDisplayName)
    {
        EnterText(_eJudgeEmail, judgeEmail);
        //WaitForElementToBeVisible(Spinner); // wait 2 seconds before search starts
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeVisible(_searchResults);
        ClickElement(_searchResults);
        if (!string.IsNullOrWhiteSpace(judgeDisplayName)) EnterText(_ejudgeDisplayNameFld, judgeDisplayName);
    }

    public ParticipantsPage GoToParticipantsPage()
    {
        WaitForElementVisible(Driver, _nextButtonEJudge);
        ClickElement(_nextButtonEJudge);
        bool useParty = true;
        return new ParticipantsPage(Driver, DefaultWaitTime, useParty);
    }
    

    public void ClickSaveEJudgeButton()
    {
        WaitForElementToBeVisible(_saveEJudge);
        ClickElement(_saveEJudge);
    }

    public void ClickNextEJudgeButton() { ClickElement(_nextButtonEJudge); }
}