namespace UI.PageModels.Pages.Admin.Booking;

public class SpecialMeasuresPage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    private readonly By _participantsToScreenDropdown = By.Id("screening-participant");
    private readonly By _participantsToScreenFromDropdown = By.XPath("//ng-select[@id='protect-participant-from']//input[@type='text']");
    private readonly By _saveButton = By.Id("confirmScreeningBtn");
    private readonly By _nextButton = By.Id("nextButton");
    
    protected override void ConfirmPageHasLoaded()
    {
        if (!Driver.Url.EndsWith("screening"))
            throw new InvalidOperationException(
                "This is not the screening page, the current url is: " + Driver.Url);
    }

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
    /// Add participants or endpoints for screening
    /// </summary>
    /// <param name="screeningParticipants"></param>
    public void AddScreeningParticipants(List<ScreeningParticipantDto> screeningParticipants)
    {
        foreach (var participant in screeningParticipants)
        {
            SelectParticipantToScreen(participant.DisplayName);
            SelectParticipantsToScreenFrom(participant.Screening.ProtectedFrom);
            ClickElement(_saveButton);
        }
    }

    private void SelectParticipantToScreen(string displayName)
    {
        WaitForDropdownListToPopulate(_participantsToScreenDropdown, 0);
        SelectDropDownByText(_participantsToScreenDropdown, displayName);
    }

    private void SelectParticipantsToScreenFrom(List<string> displayNamesToScreenFrom)
    {
        foreach (var displayNameToScreenFrom in displayNamesToScreenFrom)
        {
            ClickElement(_participantsToScreenFromDropdown);
            var checkbox = By.XPath($"//input[@aria-label='Participant display name {displayNameToScreenFrom}']");
            ClickElement(checkbox, waitToBeClickable: false);
        }
    }
}