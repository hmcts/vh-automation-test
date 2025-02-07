namespace UI.PageModels.Pages.Video.Participant;

public abstract class CommonWaitingRoomPage(IWebDriver driver, int defaultWaitTime)
    : VhVideoWebPage(driver, defaultWaitTime)
{
    public void ClearParticipantAddedNotification()
    {
        var elements = Driver.FindElements(By.XPath("//button[contains(@id,'notification-toastr-participant-added-dismiss')]"));
        foreach (var element in elements)
            element.Click();
    }
}