namespace UI.PageModels.Pages.Admin.Booking;

public class SpecialMeasuresPage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    private readonly By _nextButton = By.Id("nextButton");

    public OtherInfoPage GoToOtherInformationPage()
    {
        ClickElement(_nextButton);
        return new OtherInfoPage(Driver, DefaultWaitTime);
    }
}