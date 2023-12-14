using OpenQA.Selenium;
using UI.PageModels.Dtos;
using UI.PageModels.Extensions;

namespace UI.PageModels.Pages.Admin.Booking;

public class ParticipantsPage : VhAdminWebPage
{
    private readonly By _addParticipantLink = By.Id("addParticipantBtn");
    private readonly By _cancelButton = By.Id("cancelBtn");
    private readonly By _clearDetailsLink = By.PartialLinkText("Clear details");
    private readonly By _displayNameTextfield = By.Id("displayName");
    private readonly By _emailList = By.XPath("//ul[@id='search-results-list']//li");
    private readonly By _existingEmailLinks = By.XPath("//li[@class='vk-showlist-m30']/a");
    private readonly By _firstNameTextfield = By.Id("firstName");
    private readonly By _individualOrganisationTextfield = By.Id("companyNameIndividual");
    private readonly By _interpreteeDropdown = By.Id("interpreterFor");
    private readonly By _interpreterFor = By.Id("interpreterFor");
    private readonly By _lastNameTextfield = By.Id("lastName");

    private readonly By _newUserWarning =
        By.XPath("//*[@id='search-email-component']/app-search-email/div/div[2]/strong");

    private readonly By _nextButton = By.Id("nextButton");
    private readonly By _participantEmailTextfield = By.Id("participantEmail");
    private readonly By _participantsList = By.XPath("//*[contains(@class, 'vhtable-header')]");
    private readonly By _partyDropdown = By.Id("party");
    private readonly By _phoneTextfield = By.Id("phone");
    private readonly By _repOrganisationTextfield = By.Id("companyName");
    private readonly string _repOrganisationTextfieldId = "companyName";
    private readonly By _representingTextfield = By.Id("representing");
    private readonly By _roleDropdown = By.Id("role");
    private readonly By _titleDropdown = By.Id("title");
    private readonly By _updateParticipantLink = By.Id("updateParticipantBtn");

    public ParticipantsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeClickable(_nextButton);
        //WaitForDropdownListToPopulate(_partyDropdown);
    }

    public void AddNewIndividualParticipant(string party, string role, string contactEmail, string displayName,
        BookingNewParticipantDto bookingNewParticipantDto)
    {
        //SelectDropDownByText(_partyDropdown, party);
        SelectDropDownByText(_roleDropdown, role);
        EnterText(_participantEmailTextfield, contactEmail);
        WaitForElementToBeVisible(_newUserWarning);

        SelectDropDownByText(_titleDropdown, bookingNewParticipantDto.Title);
        EnterText(_firstNameTextfield, bookingNewParticipantDto.FirstName);
        EnterText(_lastNameTextfield, bookingNewParticipantDto.LastName);
        EnterText(_individualOrganisationTextfield, bookingNewParticipantDto.Organisation);
        EnterText(_phoneTextfield, bookingNewParticipantDto.Telephone);
        EnterText(_displayNameTextfield, displayName);
    }

    public void AddNewRepresentative(/*string party,*/ string role, string contactEmail, string displayName,
        string representing, BookingNewParticipantDto bookingNewParticipantDto)
    {
        //SelectDropDownByText(_partyDropdown, party);
        SelectDropDownByText(_roleDropdown, role);
        EnterText(_participantEmailTextfield, contactEmail);
        WaitForElementToBeVisible(_newUserWarning);

        SelectDropDownByText(_titleDropdown, bookingNewParticipantDto.Title);
        EnterText(_firstNameTextfield, bookingNewParticipantDto.FirstName);
        EnterText(_lastNameTextfield, bookingNewParticipantDto.LastName);
        EnterText(_individualOrganisationTextfield, bookingNewParticipantDto.Organisation);
        EnterText(_phoneTextfield, bookingNewParticipantDto.Telephone);
        EnterText(_displayNameTextfield, displayName);
    }

    private void AddNewParticipant(string party, string role, string contactEmail, string displayName,
        BookingNewParticipantDto bookingNewParticipantDto, string? representing = null)
    {
        //SelectDropDownByText(_partyDropdown, party);
        SelectDropDownByText(_roleDropdown, role);
        EnterText(_participantEmailTextfield, contactEmail);
        WaitForElementToBeVisible(_newUserWarning);

        SelectDropDownByText(_titleDropdown, bookingNewParticipantDto.Title);
        EnterText(_firstNameTextfield, bookingNewParticipantDto.FirstName);
        EnterText(_lastNameTextfield, bookingNewParticipantDto.LastName);
        EnterText(_individualOrganisationTextfield, bookingNewParticipantDto.Organisation);
        EnterText(_phoneTextfield, bookingNewParticipantDto.Telephone);
        EnterText(_displayNameTextfield, displayName);

        if (!string.IsNullOrWhiteSpace(representing)) EnterText(_representingTextfield, representing);

        if (HasFormValidationError())
        {
            throw new InvalidOperationException("Form validation error");
        }

        ClickAddParticipantAndWait();
    }

    public void AddExistingParticipants(List<BookingExistingParticipantDto> participants)
    {
        foreach (var participant in participants)
            switch (participant.Role)
            {
                case GenericTestRole.Representative:
                    AddExistingRepresentative(participant.Party.GetDescription(), participant.Role.GetDescription(),
                        participant.ContactEmail,
                        participant.DisplayName, participant.Representing);
                    break;
                case GenericTestRole.Witness:
                case GenericTestRole.Interpreter:
                    throw new NotImplementedException("Adding witness/interpreter is not implemented");
                default:
                    AddExistingIndividualParticipant(participant.Party.GetDescription(),
                        participant.Role.GetDescription(), participant.ContactEmail,
                        participant.DisplayName);
                    break;
            }
    }
    public void AddExistingIndividualParticipant(string? party = null, string? role = null, string? contactEmail = null, 
        string? displayName = null)
    {
        AddExistingParticipant(party, role, contactEmail, displayName);
    }
    
    public void AddExistingIndividualParticipantEjudge(string role, string contactEmail, 
        string displayName,
        string representing)
    {
        AddExistingParticipantss(role, contactEmail, displayName, representing);
    }

    public void AddExistingRepresentative(string? party = null, string? role = null, 
        string? contactEmail = null, string? displayName = null,
        string? representing = null)
    {
        AddExistingParticipant(party, role, contactEmail, displayName, representing);
    }

    private void AddExistingParticipant(string? party = null, string? role = null, string? contactEmail = null, 
        string? displayName = null,
        string? representing = null)
    {
        //SelectDropDownByText(_partyDropdown, party);
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
    
    private void AddExistingParticipantV2 ( string? role = null, string? contactEmail = null, 
        string? displayName = null,
        string? representing = null)
    {
        
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
    
    private void AddExistingParticipantssxx(string role, string contactEmail, 
        string displayName,
        string representing)
    {
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
    
    private void AddExistingParticipantss(string role, string contactEmail, string displayName, string representing)
    {
        try
        {
            WaitForElementVisible(Driver, _roleDropdown);
            SelectDropDownByText(_roleDropdown, role);
            EnterText(_participantEmailTextfield, contactEmail);

            // Wait for the email list to be clickable before clicking
            WaitForElementToBeClickable(_emailList);
            ClickElement(_emailList);

            EnterText(_displayNameTextfield, displayName);

            if (!string.IsNullOrWhiteSpace(representing))
            {
                // Wait for representingTextfield to be visible before entering text
                WaitForElementToBeVisible(_representingTextfield);
                EnterText(_representingTextfield, representing);
            }

            if (HasFormValidationError())
            {
                var message = GetValidationErrors();
                throw new InvalidOperationException($"Form has validation errors. Details: {message}");
            }

            ClickAddParticipantAndWait();
        }
        catch (Exception ex)
        {
            // Log the exception details for debugging
            Console.WriteLine($"An error occurred while adding an existing participant: {ex.Message}");
            // Re-throw the exception to maintain the original error context
            throw;
        }
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