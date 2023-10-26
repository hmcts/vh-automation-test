using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace UI.PageModels.Pages.Video.Vho;

public class CommandCentreInstantMessaging : CommandCentrePage
{    
    public CommandCentreInstantMessaging(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
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
    
    public void ValidateInstantMessagingOutboundScenario(string participantToMessageDisplayName)
    {
        // CSO will be able to IM a participant
        SelectUserFromMessagesList(participantToMessageDisplayName);
        var messageFromTheCso = "Hello from the CSO";
        SendAMessage(messageFromTheCso);
        var messagesSent = GetMessagesSent();
        if(!messagesSent.Last().Text.Contains(messageFromTheCso))
            throw new ValidationException("The message sent by the CSO was not displayed in the IM panel");
    }    
    
    public void ValidateInstantMessagingInboundScenario(string messageToCso)
    {
        // CSO will be able to receive a reply to the IM
        var messagesReceived = GetMessagesReceived();
        if(!messagesReceived.Last().Text.Contains(messageToCso))
            throw new ValidationException("The message sent by the participant was received by the CSO");
    }
}