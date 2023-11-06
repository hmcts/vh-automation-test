namespace UI.PageModels.Pages.Video.Participant;

public class StaffMemberVenueListPage : VhVideoWebPage
{
    private readonly By _venueList = By.Id("venue-allocation-list");
    private readonly By _viewHearingsBtn = By.Id("select-venue-allocation-btn");
    public StaffMemberVenueListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_venueList);
    }
    
    public StaffMemberHearingListPage SelectHearingsByVenues(params string[] venues)
    {
        var input = By.XPath("//ng-select[@id='venue-allocation-list']//input[@type='text']");
        foreach (var venue in venues)
        {
            ClickElement(input);
            EnterText(input, venue, false);
            ClickElement(By.XPath($"//input[@aria-label='Venue name {venue}']"));
        }

        ClickElement(_viewHearingsBtn);
        return new StaffMemberHearingListPage(Driver, DefaultWaitTime);
    }

}