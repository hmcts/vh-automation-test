using BookingsApi.Contract.V1.Responses;
using UI.PageModels.Pages.Video.Vho.DashboardCommandCentre;

namespace UI.PageModels.Pages.Video.Vho;

public class VhoVenueSelectionPage : VhVideoWebPage
{
    private readonly By _viewHearingsBtn = By.Id("select-venue-allocation-btn");
    public VhoVenueSelectionPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeVisible(By.XPath("//h1[normalize-space()='Select your hearing lists']"));
    }

    public CommandCentrePage SelectHearingsByVenues(params string[] venues)
    {
        var input = By.XPath("//ng-select[@id='venue-allocation-list']//input[@type='text']");
        foreach (var venue in venues)
        {
            ClickElement(input);
            EnterText(input, venue, false);
            ClickElement(By.XPath($"//input[@aria-label='Venue name {venue}']"));
        }

        ClickElement(_viewHearingsBtn);
        return new CommandCentrePage(Driver, DefaultWaitTime);
    }
    
        
    public CommandCentrePage SelectHearingsByAllocatedCso(params JusticeUserResponse[] justiceUsers)
    {
        var input = By.XPath("//ng-select[@id='cso-allocation-list']//input[@type='text']");
        foreach (var cso in justiceUsers)
        {
            ClickElement(input);
            EnterText(input, cso.FullName, false);
            ClickElement(By.XPath($"//input[@aria-label='CSO {cso.FirstName}']"));
        }

        ClickElement(_viewHearingsBtn);
        return new CommandCentrePage(Driver, DefaultWaitTime);
    }
}