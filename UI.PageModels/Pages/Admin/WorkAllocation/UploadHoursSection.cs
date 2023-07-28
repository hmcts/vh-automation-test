using OpenQA.Selenium;

namespace UI.PageModels.Pages.Admin.WorkAllocation;

public class UploadHoursSection : VhAdminWebPage
{
    private readonly By _teamWorkingHoursUploadedSuccessfully = By.CssSelector("div#file-upload-result > p");

    private readonly By _uploadAvailabilityHoursButton =
        By.CssSelector("#working-hours-file-upload-error .govuk-button");

    // Upload working hours availability
    private readonly By _uploadHoursSectionBtn = By.Id("upload-availability");

    private readonly By _uploadNonAvailabilityCsvField = By.CssSelector("#non-availability-hours-file-upload");

    private readonly By _uploadNonAvailabilityHoursButton =
        By.XPath("//div[@id='non-working-hours-file-upload-error']//button[@class='govuk-button govuk-!-margin-left-6'][normalize-space()='Upload']");

    private readonly By _uploadWorkHoursCsvFileField = By.XPath("//input[@id='working-hours-file-upload']");


    public UploadHoursSection(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public void UploadWorkHoursFile(string filePath)
    {
        ClickElement(_uploadHoursSectionBtn);
        var fullPath = Path.GetFullPath(filePath);
        AssertFileExists(filePath);
        EnterText(_uploadWorkHoursCsvFileField, fullPath);
        ClickElement(_uploadAvailabilityHoursButton);
        WaitForFileUploadSuccessMessage();
    }

    public void UploadNonWorkHoursFile(string filePath)
    {
        ClickElement(_uploadHoursSectionBtn);
        var fullPath = Path.GetFullPath(filePath);
        AssertFileExists(filePath);
        EnterText(_uploadNonAvailabilityCsvField, fullPath);
        ClickElement(_uploadNonAvailabilityHoursButton);
        WaitForFileUploadSuccessMessage();
    }
    
    private void AssertFileExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new NotFoundException($"file not found: {filePath}");
        }
    }

    public void WaitForFileUploadSuccessMessage()
    {
        WaitForElementToBeVisible(_teamWorkingHoursUploadedSuccessfully);
    }
}