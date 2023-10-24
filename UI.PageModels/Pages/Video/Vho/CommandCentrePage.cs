using System.Collections.ObjectModel;

namespace UI.PageModels.Pages.Video.Vho;

public class CommandCentrePage : VhVideoWebPage
{
    private readonly By _changeSelectionBtn = By.Id("change-venue-allocation-btn");
    private int _defaultWaitTime;

    public CommandCentrePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        _defaultWaitTime = defaultWaitTime;
    }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_changeSelectionBtn);
    }

    public VhoVenueSelectionPage ChangeVenueSelection()
    {
        ClickElement(_changeSelectionBtn);
        return new VhoVenueSelectionPage(Driver, DefaultWaitTime);
    }

    public CommandCentrePage SelectConferenceFromList(string conferenceId)
    {
        var element = By.XPath($"//div[@id='{conferenceId}-summary']");
        ClickElement(element);
        return this;
    }

    public ReadOnlyCollection<IWebElement> GetAllConferencesStartTimes() => Driver.FindElements(By.XPath("//app-vho-hearing-list//div[contains(@id, '-time')]"));
    
    public void SelectHearingByCaseNumber(string caseNumber)
    {
        ClickElement(By.XPath($"//div[normalize-space()='{caseNumber}']"));
    }

    public CommandCentreInstantMessaging ClickMessagesButton()
    {
        var element = By.XPath("//button[@id='messagesTabButton']");
        WaitForElementToBeClickable(element);
        ClickElement(element);
        return new CommandCentreInstantMessaging(Driver, _defaultWaitTime);
    }
    
    public void ClickJoinHearingButton() => ClickElement(By.XPath("//button[@id='command-centre-join-hearing-btn']"));

    public CommandCentreHearing ClickHearingsButton()
    {
        var element = By.XPath("//button[@id='hearingsTabButton']");
        WaitForElementToBeClickable(element);
        ClickElement(element);
        return new CommandCentreHearing(Driver, _defaultWaitTime);
    }

    public CommandCentrePage ReloadPage()
    {
        Driver.Navigate().Refresh();
        return this;
    } 
    
    
    public class CommandCentreHearing: VhVideoWebPage
    {
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
        
        public CommandCentreHearing(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime) { }
    }

    public class CommandCentreInstantMessaging : VhVideoWebPage
    {
        public void SelectUserFromMessagesList(string userDisplayName)
        {
            var element = By.XPath($"//div[@class='im-list']//*[contains(text(),'{userDisplayName}')]");
            WaitForElementToBeClickable(element);
            ClickElement(element);
        }
            

        public ReadOnlyCollection<IWebElement> GetMessagesSent()
        {
            var elements = By.XPath("//div[@class='messages-container']//div[contains(@class, 'message-item-sent')]");
            WaitForElementToBeVisible(elements);
            return Driver.FindElements(elements);
        }    
        public ReadOnlyCollection<IWebElement> GetMessagesReceived()
        {
            var elements = By.XPath("//div[@class='messages-container']//div[contains(@class, 'message-item-received')]");
            WaitForElementToBeVisible(elements);
            return Driver.FindElements(elements);
        }

        public void SendAMessage(string message)
        {
            var textArea = By.XPath("//textarea[@id='new-message-box']");
            EnterText(textArea, message, false);
            var sendButton = By.XPath("//button[@id='send-new-message-btn']");
            ClickElement(sendButton);
        }

        public CommandCentreInstantMessaging(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
        }
    }

   
}



