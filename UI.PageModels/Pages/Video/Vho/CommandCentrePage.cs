namespace UI.PageModels.Pages.Video.Vho;

public class CommandCentrePage : VhVideoWebPage
{
    private readonly By _changeSelectionBtn = By.Id("change-venue-allocation-btn");
    public CommandCentrePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_changeSelectionBtn);
    }

    public VhoVenueSelectionPage ChangeVenueSelection()
    {
        ClickElement(_changeSelectionBtn);
        return new VhoVenueSelectionPage(Driver, DefaultWaitTime);
    }

    public void SelectHearingByCaseNumber(string caseNumber)
    {
        ClickElement(By.XPath($"//div[normalize-space()='{caseNumber}']"));
    }
}