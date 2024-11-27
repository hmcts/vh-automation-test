using AventStack.ExtentReports;

namespace UI.PageModels.Pages.Admin.Booking;


public class ParticipantsPage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    private readonly By _addParticipantLink = By.Id("addParticipantBtn");
    private readonly By _displayNameTextfield = By.Id("displayName");
    private readonly By _emailList = By.XPath("//ul[@id='search-results-list']//li");

    private readonly By _nextButton = By.Id("nextButton");
    private readonly By _participantEmailTextfield = By.Id("participantEmail");
    private readonly string _repOrganisationTextfieldId = "companyName";
    private readonly By _representingTextfield = By.Id("representing");
    private readonly By _roleDropdown = By.Id("role");
    
    private readonly By _firstName = By.XPath("//input[@id='firstName']");
    private readonly By _lastName = By.XPath("//input[@id='lastName']");
    private readonly By _telePhone = By.XPath("//input[@id='phone']");
    private readonly By _titleDropdown = By.Id("title");
    private readonly By _confirmRemoveParticipantButton = By.Id("btn-remove");
    private readonly By _updateParticipantButton = By.Id("updateParticipantBtn");
    private readonly By _interpreterRequired = By.Name("interpreter-required");
    private readonly By _spokenLanguageDropdown = By.Id("verbal-language");
    private readonly By _signLanguageDropdown = By.Id("sign-language");


    protected override void ConfirmPageHasLoaded()
    {
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeVisible(_nextButton);
        WaitForElementToBeClickable(_nextButton);
    }

    public void AddAllParticipantsFromDto(BookingDto bookingDto)
    {
        AddParticipants(bookingDto.Participants);
        AddNewUserParticipants(bookingDto.NewParticipants);
    }
    
    public void AddNewUserParticipants(List<BookingParticipantDto> newParticipants)
    {
        foreach (var participant in newParticipants)
        {
            AddNewParticipantV2(participant);
        }
    }
    
    public void AddParticipants(List<BookingParticipantDto> participants)
    {
        foreach (var participant in participants)
        {
            AddExistingParticipantV2(participant.Title, participant.Role.ToString(), participant.ContactEmail, participant.DisplayName,
                participant.Representing, participant.InterpreterLanguage);
        }
    }

    public void UpdateParticipant(string fullName, string newDisplayName, InterpreterLanguageDto? interpreterLanguage = null)
    {
        var editLink = GetEditLink(fullName);
        ClickElement(editLink);
        WaitForDropdownListToPopulate(_roleDropdown);
        
        if (interpreterLanguage != null)
        {
            var interpreterRequiredCheckbox= FindElement(_interpreterRequired);
            if (interpreterRequiredCheckbox is { Selected: false })
            {
                ClickElement(_interpreterRequired, waitToBeClickable: false);
            }
            SelectInterpreterLanguage(interpreterLanguage);
        }
        
        EnterText(_displayNameTextfield, newDisplayName);

        ClickElementAndWaitToDisappear(_updateParticipantButton);
    }

    public void RemoveParticipant(string fullName)
    {
        var removeLink = GetRemoveLink(fullName);
        ClickElement(removeLink);
        
        ClickElementAndWaitToDisappear(_confirmRemoveParticipantButton);
    }
    
    private static By GetEditLink(string participantFullName) =>
        By.XPath($"//div[normalize-space()='{participantFullName}']/../..//a[@class='vhlink'][normalize-space()='Edit']");
    
    private static By GetRemoveLink(string participantFullName) =>
        By.XPath($"//div[normalize-space()='{participantFullName}']/../..//a[@class='vhlink'][normalize-space()='Remove']");
    
    private void AddNewParticipantV2(BookingParticipantDto newUser)
    { 
        WaitForDropdownListToPopulate(_roleDropdown, 0);
        SelectDropDownByText(_roleDropdown, newUser.Role.GetDescription());
        EnterText(_participantEmailTextfield, newUser.ContactEmail);
        WaitForDropdownListToPopulate(_titleDropdown, 0);
        SelectDropDownByText(_titleDropdown, newUser.Title);
        EnterText(_firstName, newUser.FirstName);
        EnterText(_lastName, newUser.LastName);
        EnterText(_telePhone, newUser.Phone);
        EnterText(_displayNameTextfield, newUser.DisplayName);
        
        if (HasFormValidationError())
        {
            var message = GetValidationErrors();
            throw new InvalidOperationException($"Form has validation errors.", new InvalidOperationException(message));
        }
        
        ClickAddParticipantAndWait();
        Driver.TakeScreenshotAndSave("ParticipantsPage", $"Add New Participant {newUser.ContactEmail}");
    }
    
    private void AddExistingParticipantV2(string title, string role, string contactEmail, string displayName, string? representing = null, InterpreterLanguageDto? interpreterLanguage = null)
    {
        WaitForDropdownListToPopulate(_roleDropdown, 0);
        SelectDropDownByText(_roleDropdown, role);
        EnterText(_participantEmailTextfield, contactEmail);

        ClickElement(_emailList);
        
        WaitForDropdownListToPopulate(_titleDropdown, 0);
        SelectDropDownByText(_titleDropdown, title);
        
        if (interpreterLanguage != null)
        {
            var interpreterRequiredCheckbox= FindElement(_interpreterRequired);
            if (interpreterRequiredCheckbox is { Selected: false })
            {
                ClickElement(_interpreterRequired, waitToBeClickable: false);
            }
            SelectInterpreterLanguage(interpreterLanguage);
        }
        EnterText(_displayNameTextfield, displayName);

        if (!string.IsNullOrWhiteSpace(representing)) EnterText(_representingTextfield, representing);

        if (HasFormValidationError())
        {
            var message = GetValidationErrors();
            Driver.TakeScreenshotAndSave("ParticipantsPage", $"Add Existing Participant {contactEmail}", Status.Fail);
            throw new InvalidOperationException($"Form has validation errors.", new InvalidOperationException(message));
        }
        
        ClickAddParticipantAndWait();
        Driver.TakeScreenshotAndSave("ParticipantsPage", $"Add Existing Participant {contactEmail}");
    }
    
    private void SelectInterpreterLanguage(InterpreterLanguageDto interpreterLanguage)
    {
        switch (interpreterLanguage.Type)
        {
            case InterpreterType.Sign:
                WaitForDropdownListToPopulate(_signLanguageDropdown, 0);
                SelectDropDownByText(_signLanguageDropdown, interpreterLanguage.Description);
                break;
            case InterpreterType.Verbal:
                WaitForDropdownListToPopulate(_spokenLanguageDropdown, 0);
                SelectDropDownByText(_spokenLanguageDropdown, interpreterLanguage.Description);
                break;
            default:
                throw new InvalidOperationException("Unknown interpreter language type: " + interpreterLanguage.Type);
        }
    }
    
    private void ClickAddParticipantAndWait()
    {
        ClickElementAndWaitToDisappear(_addParticipantLink);
    }

    private void ClickElementAndWaitToDisappear(By element)
    {
        Thread.Sleep(
            500); // THIS IS ABSOLUTELY REQUIRED - the component takes 500ms to respond to change based on a setTimeout
        ClickElement(element);
        WaitForElementToBeInvisible(element, 5);
        Thread.Sleep(
            500); // THIS IS ABSOLUTELY REQUIRED - the component takes 500ms to respond to change based on a setTimeout
    }

    /// <summary>
    /// Go to the next page of the booking, which is the video access points page
    /// </summary>
    /// <returns></returns>
    public VideoAccessPointsPage GoToVideoAccessPointsPage()
    {
        ClickElement(_nextButton);
        return new VideoAccessPointsPage(Driver, DefaultWaitTime);
    }
}