using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using UI.PageModels.Extensions;

namespace UI.PageModels.Pages.Admin.Booking;


public class ParticipantsPage : VhAdminWebPage
{
    private readonly By _addParticipantLink = By.Id("addParticipantBtn");
    private readonly By _displayNameTextfield = By.Id("displayName");
    private readonly By _emailList = By.XPath("//ul[@id='search-results-list']//li");

    private readonly By _nextButton = By.Id("nextButton");
    private readonly By _participantEmailTextfield = By.Id("participantEmail");
    private readonly By _partyDropdown = By.Id("party");
    private readonly string _repOrganisationTextfieldId = "companyName";
    private readonly By _representingTextfield = By.Id("representing");
    private readonly By _roleDropdown = By.Id("role");
    private readonly bool _useParty;
    private readonly By _firstName = By.XPath("//input[@id='firstName']");
    private readonly By _lastName = By.XPath("//input[@id='lastName']");
    private readonly By _telePhone = By.XPath("//input[@id='phone']");
    private readonly By _displayName = By.XPath("//input[@id='displayName']");
    private readonly By _titleDropdown = By.Id("title");


    public ParticipantsPage(IWebDriver driver, int defaultWaitTime, bool useParty) : base(driver, defaultWaitTime)
    {
        _useParty = useParty;
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeClickable(_nextButton);
        if (useParty)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(defaultWaitTime));
            wait.Until(ExpectedConditions.ElementIsVisible(_partyDropdown));
            WaitForDropdownListToPopulate(_partyDropdown);
        }
    }

    public void AddAllParticipantsFromDto(BookingDto bookingDto)
    {
        AddParticipants(bookingDto.Participants);
        AddNewUserParticipants(bookingDto.NewParticipants);
    }
    
    public void AddNewUserParticipants(List<BookingParticipantDto> newParticipants)
    {
        foreach (var participant in newParticipants)
            if(_useParty)
                AddNewParticipant(participant);
            else
                AddNewParticipantV2(participant);
    }
    
    public void AddParticipants(List<BookingParticipantDto> participants)
    {
        foreach (var participant in participants)
        {
            if(_useParty)
                AddExistingParticipant(participant.Party.GetDescription(), participant.Role.GetDescription(), participant.ContactEmail, participant.DisplayName, participant.Representing);
            else
                AddExistingParticipantV2(participant.Role.ToString(), participant.ContactEmail, participant.DisplayName, participant.Representing);
        }
    }
    
    private void AddNewParticipant(BookingParticipantDto newUser)
    {
        WaitForDropdownListToPopulate(_partyDropdown, 0);
        SelectDropDownByText(_partyDropdown, newUser.Party.GetDescription());
        WaitForDropdownListToPopulate(_roleDropdown, 0);
        SelectDropDownByText(_roleDropdown, newUser.Role.ToString());
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
    }
    
    private void AddNewParticipantV2(BookingParticipantDto newUser)
    { 
        WaitForDropdownListToPopulate(_partyDropdown, 0);
        SelectDropDownByText(_partyDropdown, newUser.Party.GetDescription());
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
    }
    
    private void AddExistingParticipant(string party, string role, string contactEmail, string displayName, string? representing = null)
    {
        SelectDropDownByText(_partyDropdown, party);
        SelectDropDownByText(_roleDropdown, role);
        EnterText(_participantEmailTextfield, contactEmail);

        ClickElement(_emailList);
        EnterText(_displayNameTextfield, displayName);

        if (!string.IsNullOrWhiteSpace(representing)) EnterText(_representingTextfield, representing);

        if (HasFormValidationError())
        {
            var message = GetValidationErrors();
            throw new InvalidOperationException($"Form has validation errors.", new InvalidOperationException(message));
        }
        
        ClickAddParticipantAndWait();
    }
    
    private void AddExistingParticipantV2(string role, string contactEmail, string displayName, string? representing = null)
    {
        WaitForDropdownListToPopulate(_roleDropdown, 0);
        SelectDropDownByText(_roleDropdown, role);
        EnterText(_participantEmailTextfield, contactEmail);

        ClickElement(_emailList);
        EnterText(_displayNameTextfield, displayName);

        if (!string.IsNullOrWhiteSpace(representing)) EnterText(_representingTextfield, representing);

        if (HasFormValidationError())
        {
            var message = GetValidationErrors();
            throw new InvalidOperationException($"Form has validation errors.", new InvalidOperationException(message));
        }
        
        ClickAddParticipantAndWait();
    }
    
    private void ClickAddParticipantAndWait()
    {
        Thread.Sleep(
            500); // THIS IS ABSOLUTELY REQUIRED - the component takes 500ms to respond to change based on a setTimeout
        ClickElement(_addParticipantLink);
        WaitForElementToBeInvisible(_addParticipantLink, 5);
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