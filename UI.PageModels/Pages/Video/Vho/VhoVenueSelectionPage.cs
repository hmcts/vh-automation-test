

namespace UI.PageModels.Pages.Video.Vho;

public class VhoVenueSelectionPage : VhVideoWebPage
{
    private readonly By _viewHearingsBtn = By.Id("select-venue-allocation-btn");
    public VhoVenueSelectionPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    public CommandCentrePage SelectHearingsByVenues(List<string> venues)
    {
        var input = By.XPath("//ng-select[@id='venue-allocation-list']//input[@type='text']");
        //ng-select[@id='venue-allocation-list']//input[@type='text']
        foreach (var venue in venues)
        {
            ClickElement(input);
            EnterText(input, venue, false);
            ClickElement(By.XPath($"//input[@aria-label='Venue name {venue}']"));
        }

        ClickElement(_viewHearingsBtn);
        return new CommandCentrePage(Driver, DefaultWaitTime);
    }
}