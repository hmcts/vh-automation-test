using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Vho;

/// <summary>
///     HearingListPage
///     Page element definitions
///     Do not add logic here
/// </summary>
public class HearingListPage
{
    public static By ConferenceList = By.ClassName("conference-list");
    public static By Quicklinks => By.CssSelector("fa-icon");
    public static By CloseButton => By.Id("closeButton");
    public static By MessagesTabButton => By.Id("messagesTabButton");
    public static By UnreadMessage => By.XPath("//div[contains(@class,'-unread-messages-image')]");
    public static By IMAvailableParticipant => By.XPath("//div[@class='name list-item available']");
    public static By MessageRecieved => By.XPath("//div[contains(@class,'message-item-received']");
    public static By ChatList => By.XPath("chat-list");
    public static By InstantMessageInput => By.Id("new-message-box");
    public static By InstantMessageButton => By.Id("send-new-message-btn");
    public static By ChatWindow => By.Id("chat-window");
    public static By VenueListItem(string hearingId) => By.XPath($"//div[@id='{hearingId}-venue']");
    public static By SummaryListItem(string hearingId) => By.XPath($"//div[@id='{hearingId}-summary']");
    public static By CaseNumberListItem(string hearingId) => By.XPath($"//div[@id='{hearingId}-case-number']");
    public static By CaseNameListItem(string hearingId) => By.XPath($"//div[@id='{hearingId}-case-name']");
    public static By DivContainsText(string text) => By.XPath($"//div[text()[contains(.,'{text}')]]");
}