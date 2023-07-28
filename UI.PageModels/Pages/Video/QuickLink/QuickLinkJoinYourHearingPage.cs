using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.QuickLink;

/// <summary>
///     This represents the Quick Link Join Your Hearing page.
///     It is used to join a hearing as a participant or observer.
///     The user only needs to provide a name and select the role they wish to join as.
/// </summary>
public class QuickLinkJoinYourHearingPage : VhVideoWebPage
{
    private readonly By _fullName = By.Id("full-name");
    private readonly By _quickLinkParticipant = By.Id("QuickLinkParticipant-item-title");
    private readonly By _quickLinkObserver = By.Id("QuickLinkObserver-item-title");
    private readonly By _continueButton = By.Id("continue-button");
    

    public QuickLinkJoinYourHearingPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    public void EnterQuickLinkUserDetails(string fullname, bool isParticipant)
    {
        EnterText(_fullName, fullname);
        ClickElement(isParticipant ? _quickLinkParticipant : _quickLinkObserver);
    }
    
    public QuickLinkHearingListPage Continue()
    {
       ClickElement(_continueButton);
       return new QuickLinkHearingListPage(Driver, DefaultWaitTime);
    }
}