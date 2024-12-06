namespace UI.PageModels.Pages.Admin.Booking;

public class HearingAssignJudgePage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    private readonly By _ejudgeDisplayNameFld = By.Id("judiciaryDisplayNameInput");
    private readonly By _eJudgeEmail = By.Id("judiciaryEmailInput");
    private readonly By _judgeInterpreterRequired = By.Name("interpreter-required");
    private readonly By _judgeSpokenLanguageDropdown = By.Id("verbal-language");
    private readonly By _judgeSignLanguageDropdown = By.Id("sign-language");
    private readonly By _nextButton = By.XPath("//*[@id='nextButtonToParticipants'] | //*[@id='nextButton']");
    private readonly By _searchResults = By.Id("search-results-list");
    private readonly By _saveEJudge = By.Id("confirmJudiciaryMemberBtn");

    protected override void ConfirmPageHasLoaded()
    {
        WaitForApiSpinnerToDisappear();
    }

    public void EnterJudgeDetails(BookingJudgeDto judge)
    {
        AssignPresidingJudiciaryDetails(judge.Username, judge.DisplayName, judge.InterpreterLanguage);
        ClickSaveJudgeButton();
        Driver.TakeScreenshotAndSave(GetType().Name, "Entered Judge Details");
    }
    
    private void AssignPresidingJudiciaryDetails(string judgeEmail, string judgeDisplayName, InterpreterLanguageDto? interpreterLanguage = null)
    {
        EnterText(_eJudgeEmail, judgeEmail);
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeVisible(_searchResults);
        ClickElement(_searchResults);
        if (!string.IsNullOrWhiteSpace(judgeDisplayName)) 
            EnterText(_ejudgeDisplayNameFld, judgeDisplayName);
        if (interpreterLanguage != null)
        {
            var interpreterRequiredCheckboxElement = Driver.FindElement(_judgeInterpreterRequired);
            if (!interpreterRequiredCheckboxElement.Selected)
            {
                ClickElement(_judgeInterpreterRequired, waitToBeClickable: false);
            }
            SelectInterpreterLanguage(interpreterLanguage);
        }
        Driver.TakeScreenshotAndSave(GetType().Name, "Entered Presiding Judiciary Details");
    }

    private void SelectInterpreterLanguage(InterpreterLanguageDto interpreterLanguage)
    {
        switch (interpreterLanguage.Type)
        {
            case InterpreterType.Sign:
                WaitForDropdownListToPopulate(_judgeSignLanguageDropdown, 0);
                SelectDropDownByText(_judgeSignLanguageDropdown, interpreterLanguage.Description);
                break;
            case InterpreterType.Verbal:
                WaitForDropdownListToPopulate(_judgeSpokenLanguageDropdown, 0);
                SelectDropDownByText(_judgeSpokenLanguageDropdown, interpreterLanguage.Description);
                break;
            default:
                throw new InvalidOperationException("Unknown interpreter language type: " + interpreterLanguage.Type);
        }
        Driver.TakeScreenshotAndSave(GetType().Name, $"Selected Interpreter Language {interpreterLanguage.Description}");
    }
    
    private void ClickSaveJudgeButton()
    {
        WaitForElementToBeVisible(_saveEJudge);
        ClickElement(_saveEJudge);
    }

    public ParticipantsPage GotToNextPage()
    {
        ClickNextButton();
        return new ParticipantsPage(Driver, DefaultWaitTime);
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