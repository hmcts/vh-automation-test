using UI.Common.CustomExceptions;

namespace UI.PageModels.Pages.Video.Vho.DashboardCommandCentre;

public class CommandCentreHearing: CommandCentrePage
{
    public CommandCentreHearing(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime) { }
    public string ParticipantStatusDisplayName(Guid participantId) => Driver.FindElement(By.XPath($"//div[@id='participant-contact-details-link-{participantId}']")).Text;

    public string ParticipantStatus(Guid participantId)
    {
        var element = By.XPath($"//p[@id='participant-status-{participantId}']");
        WaitForElementToBeVisible(element);
        return Driver.FindElement(element).Text;
    } 

    public string HearingStatus()
    {
        var element = By.XPath("//div[@class='selected-menu-content']//div[contains(@class, 'status status-button')]");
        WaitForElementToBeVisible(element);
        return Driver.FindElement(element).Text;
    } 
        
    public void ValidateParticipantStatusBeforeLogin(Guid testParticipantId)
    {
        if (ParticipantStatus(testParticipantId) != "Not signed in")
            throw new ValidationException("Participant state is not set to 'Not signed in' before login");
    }
    
    public void ValidateParticipantStatusAfterLogin( Guid testParticipantId, string conferenceId)
    {
        var participantStatus = ParticipantStatus(testParticipantId);
        //Can be slow to propagate the status change
        if (participantStatus == "Not signed in")
        {
            ReloadPage();
            SelectConferenceFromList(conferenceId);
            ClickHearingsButton();
        }
        
        if (!(ParticipantStatus(testParticipantId) == "Available" || ParticipantStatus(testParticipantId) == "Joining"))
            throw new ValidationException("Participant state is not set to 'Available' or 'Joining' after login");
    }

    public void ValidateHearingStatusBeforeStartScenario()
    {
        if (HearingStatus() != "Not Started")
            throw new ValidationException("Hearing state is not set to 'Not Started' before start");
    }
    
    public void ValidateLiveHearingStatusScenario(string conferenceId)
    {
        if(HearingStatus() == "Not Started")
        {
            ReloadPage();
            SelectConferenceFromList(conferenceId);
            ClickHearingsButton();
        }
            
        if (HearingStatus() != "In Session")
            throw new ValidationException("Hearing state is not set to 'In Session' after start");
    }   
    
    public void ValidatePausedHearingStatusScenario(string conferenceId)
    {
        if (HearingStatus() == "In Session")
        {
            ReloadPage();
            SelectConferenceFromList(conferenceId);
            ClickHearingsButton();
        }
        
        if (HearingStatus() != "Paused")
            throw new ValidationException("Hearing state is not set to 'Paused' after pause");
    }
}