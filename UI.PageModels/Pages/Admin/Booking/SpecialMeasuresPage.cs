namespace UI.PageModels.Pages.Admin.Booking;

public class SpecialMeasuresPage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    private readonly By _participantsAndEndpointsToScreenDropdown = By.Id("screening-participant");
    private readonly By _participantsAndEndpointsToScreenFromDropdown = By.XPath("//ng-select[@id='protect-participant-from']//input[@type='text']");
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

    public void ScreenParticipant(ScreeningParticipantDto screeningParticipantDto)
    {
        SelectParticipantOrEndpointToScreen(screeningParticipantDto);
        SelectParticipantsOrEndpointsToScreenFrom(screeningParticipantDto);
        ClickElement(_saveButton);
    }

    private void SelectParticipantOrEndpointToScreen(ScreeningParticipantDto screeningParticipantDto)
    {
        WaitForDropdownListToPopulate(_participantsAndEndpointsToScreenDropdown, 0);
        SelectDropDownByText(_participantsAndEndpointsToScreenDropdown, screeningParticipantDto.DisplayName);
    }

    private void SelectParticipantsOrEndpointsToScreenFrom(ScreeningParticipantDto screeningParticipantDto)
    {
        foreach (var displayNameToScreenFrom in screeningParticipantDto.DisplayNamesToScreenFrom)
        {
            ClickElement(_participantsAndEndpointsToScreenFromDropdown);
            var checkbox = By.XPath($"//input[@aria-label='Participant display name {displayNameToScreenFrom}']");
            ClickElement(checkbox, waitToBeClickable: false);
        }
    }
}