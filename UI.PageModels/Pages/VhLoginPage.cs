using OpenQA.Selenium;

namespace UI.PageModels.Pages;

public abstract class VhLoginPage : VhPage
{
    private readonly By _usernameTextfield = By.Id("i0116");
    private readonly By _passwordField = By.Id("i0118");
    private readonly By _nextBtn = By.Id("idSIButton9");
    private readonly By _signInBtn = By.Id("idSIButton9"); // also the next button

    protected VhLoginPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
		
    protected void EnterLoginDetails(string username, string password)
    {
        EnterText(_usernameTextfield, username);
        ClickElement(_nextBtn);
			
        EnterText(_passwordField, password);
        ClickElement(_signInBtn);
    }
}