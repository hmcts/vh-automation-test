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

    public void ScreenParticipant(ScreeningParticipantDto screeningParticipantDto)
    {
        SelectParticipantToScreen(screeningParticipantDto);
        SelectParticipantsToScreenFrom(screeningParticipantDto);
        ClickElement(_saveButton);
    }

    private void SelectParticipantToScreen(ScreeningParticipantDto screeningParticipantDto)
    {
        WaitForDropdownListToPopulate(_participantsToScreenDropdown, 0);
        SelectDropDownByText(_participantsToScreenDropdown, screeningParticipantDto.DisplayName);
    }

    private void SelectParticipantsToScreenFrom(ScreeningParticipantDto screeningParticipantDto)
    {
        foreach (var displayNameToScreenFrom in screeningParticipantDto.DisplayNamesToScreenFrom)
        {
            ClickElement(_participantsToScreenFromDropdown);
            var checkbox = By.XPath($"//input[@aria-label='Participant display name {displayNameToScreenFrom}']");
            ClickElement(checkbox, waitToBeClickable: false);
        }
    }
}