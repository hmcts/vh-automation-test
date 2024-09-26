namespace UI.PageModels.Pages.Admin.Booking;

public class SpecialMeasuresPage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    private readonly By _nextButton = By.Id("nextButton");

    public OtherInfoPage GoToOtherInformationPage()
    {
        ClickElement(_nextButton);
        return new OtherInfoPage(Driver, DefaultWaitTime);
    }
    
    /// <summary>
    /// When in edit mode, the next button directs a user to the summary page
    /// </summary>
    /// <returns></returns>
    public SummaryPage GoToSummaryPage()
    {
        ClickElement(_nextButton);
        return new SummaryPage(Driver, DefaultWaitTime);
    }
}