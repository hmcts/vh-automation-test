using OpenQA.Selenium;

namespace UI.PageModels.Pages.Admin;

public class AdminWebLoginPage : VhLoginPage
{
    private readonly By _nextBtn = By.Id("idSIButton9");
    private readonly By _passwordField = By.Id("i0118");
    private readonly By _signInBtn = By.Id("idSIButton9"); // also the next button

    private readonly By _usernameTextfield = By.Id("i0116");

    public AdminWebLoginPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(_nextBtn);
    }

    public DashboardPage Login(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new DashboardPage(Driver, DefaultWaitTime);
    }
}