using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

/// <summary>
///     Which page is this?
/// </summary>
public class SignInToHearingPage : VhVideoWebPage
{
    public SignInToHearingPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public static By CheckEquipmentButton => By.Id("check-equipment-btn");
    public static By SignInToHearingButton => By.XPath("//button[contains(@id,'sign-into-hearing-btn-')]");
}