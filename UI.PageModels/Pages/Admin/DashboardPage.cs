using OpenQA.Selenium;
using UI.PageModels.Pages.Admin.Booking;
using UI.PageModels.Pages.Admin.WorkAllocation;

namespace UI.PageModels.Pages.Admin;

public class DashboardPage : VhAdminWebPage
{
    private readonly By _bookHearingButton = By.Id("bookHearingBtn");
    private readonly By _unallocatedHearingsNextSevenDays = By.Id("unallocated-hearings-next-seven-days");
    private readonly By _unallocatedHearingsNextThirtyDays = By.Id("unallocated-hearings-next-thirty-days");

    private readonly By _unallocatedHearingsToday = By.Id("unallocated-hearings-today");
    private readonly By _unallocatedHearingsTomorrow = By.Id("unallocated-hearings-tomorrow");
    private readonly By _workAllocationButton = By.Id("manageWorkAllocationBtn");

    public DashboardPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
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
        return new HearingDetailsPage(Driver, DefaultWaitTime);
    }

    public ManageWorkAllocationPage GoToManageWorkAllocation()
    {
        WaitForApiSpinnerToDisappear();
        ClickElement(_workAllocationButton);
        return new ManageWorkAllocationPage(Driver, DefaultWaitTime);
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