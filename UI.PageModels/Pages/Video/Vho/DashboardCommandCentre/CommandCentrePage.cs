using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace UI.PageModels.Pages.Video.Vho;

public class CommandCentrePage : VhVideoWebPage
{
    private readonly By _changeSelectionBtn = By.Id("change-venue-allocation-btn");
    private int _defaultWaitTime;

    public CommandCentrePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        _defaultWaitTime = defaultWaitTime;
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

    public void SelectConferenceFromList(string conferenceId)
    {
        var element = By.XPath($"//div[@id='{conferenceId}-summary']");
        ClickElement(element);
    }

    public ReadOnlyCollection<IWebElement> GetAllConferencesStartTimes()
    {
        var conferences = By.XPath("//app-vho-hearing-list//div[contains(@id, '-time')]");
        WaitForElementToBeVisible(conferences);
        return Driver.FindElements(conferences);
    } 
    
    public void SelectHearingByCaseNumber(string caseNumber)
    {
        ClickElement(By.XPath($"//div[normalize-space()='{caseNumber}']"));
    }

    public CommandCentreInstantMessaging ClickMessagesButton()
    {
        var element = By.XPath("//button[@id='messagesTabButton']");
        WaitForElementToBeClickable(element);
        ClickElement(element);
        return new CommandCentreInstantMessaging(Driver, _defaultWaitTime);
    }
    
    public void ClickJoinHearingButton() => ClickElement(By.XPath("//button[@id='command-centre-join-hearing-btn']"));

    public CommandCentreHearing ClickHearingsButton()
    {
        var element = By.XPath("//button[@id='hearingsTabButton']");
        WaitForElementToBeClickable(element);
        ClickElement(element);
        return new CommandCentreHearing(Driver, _defaultWaitTime);
    }

    public void ReloadPage()
    {
        Driver.Navigate().Refresh();
    } 
    
    public void ValidateHearingsAreInChronologicalOrder()
    {
        var conferences = GetAllConferencesStartTimes();
        var hearingStartTimes = conferences
            .Select(x => DateTime.ParseExact(x.Text, "HH:mm", CultureInfo.InvariantCulture))
            .ToList();
        var orderedStartTimes = hearingStartTimes.OrderBy(x => x).ToList();
        for (var i = 0; i < hearingStartTimes.Count; i++)
            if (hearingStartTimes[i] != orderedStartTimes[i])
                throw new ValidationException("Hearings are not in chronological order");
                
    }

}



