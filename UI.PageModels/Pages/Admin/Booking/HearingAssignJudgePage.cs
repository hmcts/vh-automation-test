namespace UI.PageModels.Pages.Admin.Booking;

public class HearingAssignJudgePage : VhAdminWebPage
{
    private readonly By _judgeDisplayNameFld = By.Id("judgeDisplayNameFld");
    private readonly By _ejudgeDisplayNameFld = By.Id("judiciaryDisplayNameInput");
    private readonly By _judgeEmail = By.Id("judge-email");
    private readonly By _eJudgeEmail = By.Id("judiciaryEmailInput");
    private readonly By _judgePhoneFld = By.Id("judgePhoneFld");
    private readonly By _nextButton = By.XPath("//*[@id='nextButtonToParticipants'] | //*[@id='nextButton']");
    private readonly By _searchResults = By.Id("search-results-list");
    private readonly By _saveEJudge = By.Id("confirmJudiciaryMemberBtn");
    private readonly By _contWithoutJOH = By.XPath("//button[@id='nextButtonToParticipants']");
    

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
        if (!string.IsNullOrWhiteSpace(judgeDisplayName)) 
            EnterText(_judgeDisplayNameFld, judgeDisplayName);

        if (!string.IsNullOrWhiteSpace(judgePhone)) 
            EnterText(_judgePhoneFld, judgePhone);
    }
    
    public void AssignPresidingJudiciaryDetails(string judgeEmail, string judgeDisplayName)
    {
        EnterText(_eJudgeEmail, judgeEmail);
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeVisible(_searchResults);
        ClickElement(_searchResults);
        if (!string.IsNullOrWhiteSpace(judgeDisplayName)) 
            EnterText(_ejudgeDisplayNameFld, judgeDisplayName);
    }
    
    
    public void ClickSaveJudgeButton()
    {
        WaitForElementToBeVisible(_saveEJudge);
        ClickElement(_saveEJudge);
    }

    public ParticipantsPage GotToNextPage(bool isV2)
    {
        var useParty = !isV2;
        ClickNextButton();
        return new ParticipantsPage(Driver, DefaultWaitTime, useParty);
    }


    public SummaryPage GotToNextPageOnEdit()
    {
        ClickNextButton();
        return new SummaryPage(Driver, DefaultWaitTime);
    }
    
    private void ClickNextButton()
    {
        WaitForElementToBeVisible(_nextButton);
        ClickElement(_nextButton);
    }
}