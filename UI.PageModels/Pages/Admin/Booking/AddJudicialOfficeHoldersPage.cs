namespace UI.PageModels.Pages.Admin.Booking;

public class AddJudicialOfficeHoldersPage : VhAdminWebPage
{
    private readonly By _presidingJudgeEmailInput =
        By.XPath("//*[@id='searchForPresidingJudge']//input[@id='judiciaryEmailInput']");
    private readonly By _presidingJudgeDisplayNameInput =
        By.XPath("//*[@id='searchForPresidingJudge']//input[@id='judiciaryDisplayNameInput']");
    private readonly By _presidingJudgeSaveButton =
        By.XPath("//*[@id='searchForPresidingJudge']//button[@id='confirmJudiciaryMemberBtn']");
    
    private readonly By _panelMemberEmailInput =
        By.XPath("//*[@id='searchForPanelMember']//input[@id='judiciaryEmailInput']");
    private readonly By _panelMemberDisplayNameInput =
        By.XPath("//*[@id='searchForPanelMember']//input[@id='judiciaryDisplayNameInput']");
    private readonly By _panelMemberSaveButton =
        By.XPath("//*[@id='searchForPanelMember']//button[@id='confirmJudiciaryMemberBtn']");
    
    private readonly By _searchResults = By.Id("search-results-list");
    
    public AddJudicialOfficeHoldersPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    /// <summary>
    /// Set the presiding judge details
    /// </summary>
    /// <param name="email"></param>
    /// <param name="displayName"></param>
    public void EnterPresidingJudgeDetails(string email, string displayName)
    {
        EnterText(_presidingJudgeEmailInput, email);
        WaitForElementToBeVisible(Spinner); // wait 2 seconds before search starts
        WaitForApiSpinnerToDisappear();
       
        WaitForElementToBeVisible(_searchResults);
        ClickElement(_searchResults);
        
        EnterText(_presidingJudgeDisplayNameInput, displayName);
        
        WaitForElementToBeVisible(_presidingJudgeSaveButton);
        ClickElement(_presidingJudgeSaveButton);
    }

    /// <summary>
    /// Click the add panel member button and enter the details
    /// </summary>
    /// <param name="email"></param>
    /// <param name="displayName"></param>
    public void EnterPanelMemberDetails(string email, string displayName)
    {
        ClickElement(By.Id("addAdditionalPanelMemberBtn"));
        EnterText(_panelMemberEmailInput, email);
        WaitForElementToBeVisible(Spinner); // wait 2 seconds before search starts
        WaitForApiSpinnerToDisappear();
       
        WaitForElementToBeVisible(_searchResults);
        ClickElement(_searchResults);
        
        EnterText(_panelMemberDisplayNameInput, displayName);
        
        WaitForElementToBeVisible(_panelMemberSaveButton);
        ClickElement(_panelMemberSaveButton);
    }
    
    /// <summary>
    /// Go to the participants page.
    /// Note: if this booking is in edit mode, the next button will navigate to the summary page instead 
    /// </summary>
    /// <returns></returns>
    // public ParticipantsPage GoToParticipantsPage()
    // {
    //     ClickElement(By.Id("nextButtonToParticipants"));
    //     return new ParticipantsPage(Driver, DefaultWaitTime);
    // }

    
}