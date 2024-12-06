using OpenQA.Selenium;

namespace UI.PageModels.Pages.Admin;

public class AdminWebLoginPage(IWebDriver driver, int defaultWaitTime) : VhLoginPage(driver, defaultWaitTime)
{
    private readonly By _nextBtn = By.Id("idSIButton9");

    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(_nextBtn);
    }

    public DashboardPage Login(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new DashboardPage(Driver, DefaultWaitTime);
    }
}