using UI.PageModels.Pages.Admin.Booking;
using UI.PageModels.Pages.Admin.WorkAllocation;

namespace UI.PageModels.Pages.Admin;

public class DashboardPage(IWebDriver driver, int defaultWaitTime) : VhAdminWebPage(driver, defaultWaitTime)
{
    private readonly By _bookHearingButton = By.Id("bookHearingBtn");
    private readonly By _unallocatedHearingsNextSevenDays = By.Id("unallocated-hearings-next-seven-days");
    private readonly By _unallocatedHearingsNextThirtyDays = By.Id("unallocated-hearings-next-thirty-days");
    private readonly By _unallocatedHearingsToday = By.Id("unallocated-hearings-today");
    private readonly By _unallocatedHearingsTomorrow = By.Id("unallocated-hearings-tomorrow");
    private readonly By _workAllocationButton = By.Id("manageWorkAllocationBtn");
    private readonly By _manageTeamButton = By.Id("manageTeamBtn");
    private readonly By _changePasswordButton = By.Id("changePasswordBtn");

    protected override void ConfirmPageHasLoaded()
    {
        WaitForApiSpinnerToDisappear();
        WaitForElementToBeClickable(_bookHearingButton);
        if (!Driver.Url.EndsWith("dashboard"))
            throw new InvalidOperationException(
                "This is not the dashboard page, the current url is: " + Driver.Url);
    }

    public int GetNumberOfUnallocatedHearingsToday()
    {
        WaitForApiSpinnerToDisappear();
        return GetNumberOfUnallocatedHearings(_unallocatedHearingsToday);
    }

    public int GetNumberOfUnallocatedHearingsTomorrow()
    {
        WaitForApiSpinnerToDisappear();
        return GetNumberOfUnallocatedHearings(_unallocatedHearingsTomorrow);
    }

    public int GetNumberOfUnallocatedHearingsNextSevenDays()
    {
        WaitForApiSpinnerToDisappear();
        return GetNumberOfUnallocatedHearings(_unallocatedHearingsNextSevenDays);
    }

    public int GetNumberOfUnallocatedHearingsNextThirtyDays()
    {
        WaitForApiSpinnerToDisappear();
        return GetNumberOfUnallocatedHearings(_unallocatedHearingsNextThirtyDays);
    }

    public HearingDetailsPage GoToBookANewHearing()
    {
        WaitForApiSpinnerToDisappear();
        ClickElement(_bookHearingButton);
        Driver.TakeScreenshotAndSave(GetType().Name, "Clicked Book Hearing Button");
        return new HearingDetailsPage(Driver, DefaultWaitTime);
    }

    public ManageWorkAllocationPage GoToManageWorkAllocation()
    {
        WaitForApiSpinnerToDisappear();
        ClickElement(_workAllocationButton);
        Driver.TakeScreenshotAndSave(GetType().Name, "Clicked Manage Work Allocation Button");
        return new ManageWorkAllocationPage(Driver, DefaultWaitTime);
    }
    
    public ManageTeamPage GoToManageTeam()
    {
        WaitForApiSpinnerToDisappear();
        ClickElement(_manageTeamButton);
        Driver.TakeScreenshotAndSave(GetType().Name, "Clicked Manage Team Button");
        return new ManageTeamPage(Driver, DefaultWaitTime);
    }
    
    public ChangePasswordPage GoToChangePassword()
    {
        WaitForApiSpinnerToDisappear();
        ClickElement(_changePasswordButton);
        Driver.TakeScreenshotAndSave(GetType().Name, "Clicked Change Password Button");
        return new ChangePasswordPage(Driver, DefaultWaitTime);
    }
    
    private int GetNumberOfUnallocatedHearings(By locator)
    {
        var text = GetText(locator);
        if (!int.TryParse(text, out var count))
            throw new InvalidDataException(
                $"Unable to parse the number of unallocated hearings today from the text value {locator.Criteria}");

        return count;
    }
}