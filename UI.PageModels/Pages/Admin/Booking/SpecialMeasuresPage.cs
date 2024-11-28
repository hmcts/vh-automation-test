namespace UI.PageModels.Pages.Admin.Booking;

public class SpecialMeasuresPage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    protected override void ConfirmPageHasLoaded()
    {
        if (!Driver.Url.EndsWith("screening"))
            throw new InvalidOperationException(
                "This is not the screening page, the current url is: " + Driver.Url);
    }

    private readonly By _nextButton = By.Id("nextButton");

    /// <summary>
    /// On a new booking, clicking next will take you to the other information page
    /// </summary>
    /// <returns></returns>
    public OtherInfoPage GoToOtherInformationPage()
    {
        ClickElement(_nextButton);
        return new OtherInfoPage(Driver, DefaultWaitTime);
    }

    /// <summary>
    /// When in edit mode, return to the summary page after clicking next
    /// </summary>
    /// <returns></returns>
    public SummaryPage GoToSummaryPage()
    {
        ClickElement(_nextButton);
        return new SummaryPage(Driver, DefaultWaitTime);
    }
}