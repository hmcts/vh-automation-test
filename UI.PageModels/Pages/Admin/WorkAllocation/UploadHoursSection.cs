using FluentAssertions;
using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.WorkAllocation;

public class UploadHoursSection : VhAdminWebPage
{
    // Upload working hours availability
    private readonly By _uploadHoursSectionBtn = By.Id("upload-availability");

    private readonly By _uploadWorkHoursCsvFileField = By.XPath("//input[@id='working-hours-file-upload']");

    private readonly By _uploadAvailabilityHoursButton =
        By.CssSelector("#working-hours-file-upload-error .govuk-button");

    private readonly By _uploadNonAvailabilityCsvField = By.CssSelector("#non-availability-hours-file-upload");

    private readonly By _uploadNonAvailabilityHoursButton =
        By.XPath("//div[@id='non-working-hours-file-upload-error']//button[.='Upload']");

    private readonly By _teamWorkingHoursUploadedSuccessfully = By.CssSelector("div#file-upload-result > p");


    public UploadHoursSection(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public void UploadWorkHoursFile(string filePath)
    {
        ClickElement(_uploadHoursSectionBtn);
        var fullPath = Path.GetFullPath(filePath);
        File.Exists(filePath).Should().BeTrue();
        EnterText(_uploadWorkHoursCsvFileField, fullPath);
        ClickElement(_uploadAvailabilityHoursButton);
        WaitForFileUploadSuccessMessage();
    }

    public void UploadNonWorkHoursFile(string filePath)
    {
        ClickElement(_uploadHoursSectionBtn);
        var fullPath = Path.GetFullPath(filePath);
        File.Exists(filePath).Should().BeTrue();
        EnterText(_uploadNonAvailabilityCsvField, fullPath);
        ClickElement(_uploadNonAvailabilityHoursButton);
        WaitForFileUploadSuccessMessage();
    }

    public void WaitForFileUploadSuccessMessage()
    {
        WaitForElementToBeVisible(_teamWorkingHoursUploadedSuccessfully);
    }
}