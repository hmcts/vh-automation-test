namespace UI.PageModels.Pages.Video.Participant;

public class ParticipantWaitingRoomPage : VhVideoWebPage
{
    public static string ParticipantWaitingRoomClosedTitle = "This hearing has finished. You may now sign out";
    public static string ParticipantWaitingRoomPausedTitle = "The judge will restart the hearing when they are ready. Please stay near your screen";
    private readonly By _chooseCameraAndMicButton = By.Id("changeCameraButton");
    private readonly By _startPrivateConsultationBtn = By.Id("openStartPCButton");
    private readonly By _joinPrivateMeetingButton = By.Id("openJoinPCButton");
    private readonly By _acceptConsultationButton = By.Id("notification-toastr-invite-accept");
    private readonly By _continueJoiningPrivateMeetingButton = By.Id("continue-btn");
    private readonly By openChatButton = By.XPath("//img[@id='open-chat-arrow']");
    private readonly By closeChatButton = By.XPath("//img[@id='close-chat-arrow']");
    private readonly By _selectHearing =By.Id ("startHearingButton");
    public ParticipantWaitingRoomPage(IWebDriver driver, int defaultWaitTime, bool isParticipantWithLimitedControls = false) : base(driver, defaultWaitTime, isParticipantWithLimitedControls)
    {
    }

    protected override void ConfirmPageHasLoaded()
    {
        if (UseAltLocator)
            WaitForElementToBeVisible(_chooseCameraAndMicButton, 10);
        else
            WaitForElementToBeClickable(_startPrivateConsultationBtn, DefaultWaitTime);
    }

    private readonly By StartVideoHearingButton = By.XPath("//button[contains(text(),'Start video hearing')]");
    
    private readonly By ConfirmStartButton = By.Id("btnConfirmStart");
    private readonly By CancelStartHearingButton = By.Id("btnCancelStart");
    private readonly By HearingClosedTitle = By.XPath("//h1[contains(text(),'This hearing has finished. You may now sign out')]");
    private readonly By PrivateMeetingModal = By.XPath("//div[@class='modal-content']");
    private readonly By ChooseCameraAndMicButton = By.Id("changeCameraButton");
    
    
    private readonly By NotSignedInStatus = By.XPath("//label[contains(@class,'label-status--not_signed_in')]");
    private readonly By ConnectedStatus = By.XPath("//label[contains(@class,'label-status--available')]");
    private readonly By UnAvailableStatus = By.XPath("//label[contains(@class,'label-status--unavailable')]");
    private readonly By DisconnectedStatus = By.XPath("//label[contains(@class,'label-status--disconnected')]");
    private readonly By Returntovideohearinglist = By.Id("back-link");
    // private readonly By ParticipantDetails(string name) = By.XPath($"//*[contains(text(),'{name}')]");
    // private readonly By JointPrivateMeetingCheckbox(string name) = By.XPath($"//div[contains(.,'{name}')]//input");

    public ConsultationRoomPage StartPrivateConsultation(List<string> displayNames)
    {
        ClickElement(_startPrivateConsultationBtn);
        foreach (var name in displayNames)
        {
            var unavailableTag =
                By.XPath(
                    $"//span[normalize-space()='{name}']/following-sibling::label[contains(@class,'label-status--unavailable')]");
            WaitForElementToBeInvisible(unavailableTag, DefaultWaitTime);
            ClickElement(By.XPath($"//span[normalize-space()='{name}']"));
        }
        ClickElement(_continueJoiningPrivateMeetingButton);
        return new ConsultationRoomPage(Driver, DefaultWaitTime);
    }

    public ConsultationRoomPage AcceptPrivateConsultation()
    {
        ClickElement(_acceptConsultationButton);
        return new ConsultationRoomPage(Driver, DefaultWaitTime);
    }

    public ParticipantHearingRoomPage TransferToHearingRoom()
    {
        return new ParticipantHearingRoomPage(Driver, DefaultWaitTime);
    }
    public ParticipantWaitingRoomPage OpenChatWithVHO()
    {
        if(!IsElementVisible(closeChatButton) && IsElementVisible(openChatButton))
            ClickElement(openChatButton);
        return this;
    } 
    
    public ParticipantWaitingRoomPage SendAMessageToVHO(string message)
    {
        var textArea = By.XPath("//textarea[@id='new-message-box']");
        EnterText(textArea, message, false);
        var sendButton = By.XPath("//button[@id='send-new-message-btn']");
        ClickElement(sendButton);
        return this;
    }
    public PanelMemberWaitingRoomPage StartHearing()
    {
        ClickElement(_selectHearing);
        return new PanelMemberWaitingRoomPage(Driver, DefaultWaitTime);
    }
   
   

}