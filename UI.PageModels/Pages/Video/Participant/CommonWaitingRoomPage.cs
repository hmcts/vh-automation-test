namespace UI.PageModels.Pages.Video.Participant;

public abstract class CommonWaitingRoomPage
    : VhVideoWebPage
{
    protected CommonWaitingRoomPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        
    }

    protected CommonWaitingRoomPage(IWebDriver driver, int defaultWaitTime,
        bool isParticipantWithLimitedControls = false) : base(driver, defaultWaitTime, isParticipantWithLimitedControls)
    {

    }

    public void ClearParticipantAddedNotification()
    {
        var elements = Driver.FindElements(By.XPath("//button[contains(@id,'notification-toastr-participant-added-dismiss')]"));
        foreach (var element in elements)
            element.Click();
    }
}