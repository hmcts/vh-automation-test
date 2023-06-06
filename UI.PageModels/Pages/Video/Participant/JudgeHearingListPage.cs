using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class JudgeHearingListPage : VhVideoWebPage 
{
    private readonly By _checkEquipmentBtn = By.Id("check-equipment-btn");

    public JudgeHearingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(_checkEquipmentBtn);
    }

    public JudgeWaitingRoomPage SelectHearing(string caseName)
    {
        var selectHearingLocator = By.XPath($"//tr[contains(.,'{caseName}')]//button");
        ClickElement(selectHearingLocator);
        return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
    }
}