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

    public void AddParticipants(List<BookingExistingParticipantDto> participants)
    {
        foreach (var participant in participants)
        {
            if(_useParty)
                AddExistingParticipant(participant.Party.GetDescription(), participant.Role.GetDescription(), participant.ContactEmail, participant.DisplayName, participant.Representing);
            else
                AddExistingParticipantV2(participant.Role.ToString(), participant.ContactEmail, participant.DisplayName, participant.Representing);
        }
    }
    
    

    public void AddNewParticipantsWithGeneratedData(List<BookingNewParticipantDto> hearingData)
    { 
        EnterText(_participantEmailTextfield, hearingData.);

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
    private void AddNewParticipantV2(string role, string contactEmail, string displayName, string? representing = null)
    {
        WaitForDropdownListToPopulate(_roleDropdown, 0);
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