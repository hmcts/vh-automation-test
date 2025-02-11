using UI.PageModels.Pages.Video.Participant;
namespace UI.PageModels.Pages.Admin.Booking;

public class HearingAssignJudgePage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    private readonly By _ejudgeDisplayNameFld = By.Id("judiciaryDisplayNameInput");
    private readonly By _eJudgeEmail = By.Id("judiciaryEmailInput");
    private readonly By _judgeInterpreterRequired = By.Name("interpreter-required");
    private readonly By _judgeSpokenLanguageDropdown = By.Id("verbal-language");
    private readonly By _judgeSignLanguageDropdown = By.Id("sign-language");
    private readonly By _saveEJudge = By.Id("confirmJudiciaryMemberBtn");

    private readonly By _addJudicialOfficeHolder = By.Id("addAdditionalPanelMemberBtn");
    private readonly By _panelMemberDisplayNameFld = By.XPath("(//input[@id='judiciaryDisplayNameInput'])[2]");
    private readonly By _panelMemberEmail = By.XPath("(//input[@id='judiciaryEmailInput'])[2]");
    private readonly By _panelMemberInterpreterRequired = By.Name("interpreter-required");
    private readonly By _panelMemberSpokenLanguageDropdown = By.Id("verbal-language");
    private readonly By _panelMemberSignLanguageDropdown = By.Id("sign-language");
    private readonly By _savePanelMember = By.Id("confirmJudiciaryMemberBtn");
    

    private readonly By _nextButton = By.XPath("//*[@id='nextButtonToParticipants'] | //*[@id='nextButton']");
    private readonly By _searchResults = By.Id("search-results-list");
   

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

    private void AssignPresidingJudiciaryDetails(string judgeEmail, string judgeDisplayName,
        InterpreterLanguageDto? interpreterLanguage = null)
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

        Driver.TakeScreenshotAndSave(GetType().Name,
            $"Selected Interpreter Language {interpreterLanguage.Description}");
    }

    private void ClickSaveJudgeButton()
    {
        WaitForElementToBeVisible(_saveEJudge);
        ClickElement(_saveEJudge);
    }

    public void clickAddJohLink ()
    {
        ClickElement(_addJudicialOfficeHolder);
    }
    public void EnterPanelMemberDetails(BookingPanelMemberDto panelMember)
    {
        AssignPanelMemberDetails(panelMember.Username, panelMember.DisplayName, panelMember.InterpreterLanguage);
        ClickSavePanelMemberButton();
        Driver.TakeScreenshotAndSave(GetType().Name, "Enter panelMember Details");
    }

    private void AssignPanelMemberDetails(string PanelMemberEmail, string PanelanelMemberDisplayName,
        InterpreterLanguageDto? interpreterLanguage = null)
    {
        EnterText(_panelMemberEmail, PanelMemberEmail);
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeVisible(_searchResults);
        ClickElement(_searchResults);
        if (!string.IsNullOrWhiteSpace(PanelanelMemberDisplayName))
            EnterText(_panelMemberDisplayNameFld, PanelanelMemberDisplayName);

        Driver.TakeScreenshotAndSave(GetType().Name, "Enter PanelMember Details");
    }
    

    void SelectInterpreterLangugage(InterpreterLanguageDto interpreterLanguage)
    { 
        switch (interpreterLanguage.Type)
        {
            case InterpreterType.Sign:
                WaitForDropdownListToPopulate(_panelMemberSignLanguageDropdown, 0);
                SelectDropDownByText(_panelMemberSignLanguageDropdown, interpreterLanguage.Description);
                break;
            case InterpreterType.Verbal:
                WaitForDropdownListToPopulate(_panelMemberSpokenLanguageDropdown, 0);
                SelectDropDownByText(_panelMemberSpokenLanguageDropdown, interpreterLanguage.Description);
                break;
            default:
                throw new InvalidOperationException("Unknown interpreter language type: " + interpreterLanguage.Type);
        }

        Driver.TakeScreenshotAndSave(GetType().Name,
            $"Selected Interpreter Language {interpreterLanguage.Description}");
    }

    public void ClickSavePanelMemberButton()
    {
        WaitForElementToBeVisible(_savePanelMember);
        ClickElement(_savePanelMember);
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

    public void ClickNextButton()
    {
        WaitForElementToBeVisible(_nextButton);
        ClickElement(_nextButton);
    }
}