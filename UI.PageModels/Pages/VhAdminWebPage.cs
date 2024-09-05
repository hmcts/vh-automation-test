using UI.PageModels.Pages.Admin;

namespace UI.PageModels.Pages;

public abstract class VhAdminWebPage : VhPage
{
    private readonly By _bookingListBMenuItemButton = By.Id("topItem1");
    private readonly By _dashboardMenuItemButton = By.Id("topItem0");
    private readonly By _signOutMenuItemButton = By.Id("linkSignOut");

    protected VhAdminWebPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime, useAltLocator:false, ignoreAccessibilityForPage: true)
    {
    }

    public DashboardPage GoToDashboardPage()
    {
        WaitForApiSpinnerToDisappear();
        ClickElement(_dashboardMenuItemButton);
        return new DashboardPage(Driver, DefaultWaitTime);
    }

    public BookingListPage GoToBookingList()
    {
        WaitForApiSpinnerToDisappear();
        ClickElement(_bookingListBMenuItemButton);
        return new BookingListPage(Driver, DefaultWaitTime);
    }


    public void SignOut()
    {
        WaitForElementToBeVisible(_signOutMenuItemButton);
        ClickElement(_signOutMenuItemButton);
        Driver.FindElement(_signOutMenuItemButton).Click();
    }
}