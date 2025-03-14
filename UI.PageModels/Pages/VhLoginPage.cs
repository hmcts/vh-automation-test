namespace UI.PageModels.Pages;

public abstract class VhLoginPage : VhPage
{
    private readonly By _nextBtn = By.Id("idSIButton9");
    private readonly By _passwordField = By.Id("i0118");
    private readonly By _signInBtn = By.Id("idSIButton9"); // also the next button
    private readonly By _usernameTextfield = By.Id("i0116");
    private readonly By _currentPasswordField = By.Id("currentPassword");
    private readonly By _newPasswordField = By.Id("newPassword");
    private readonly By _confirmNewPasswordField = By.Id("confirmNewPassword");
    protected VhLoginPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime, useAltLocator:false)
    {
    }

    protected void EnterLoginDetails(string username, string password)
    {
        EnterText(_usernameTextfield, username);
        ClickElement(_nextBtn);

        EnterText(_passwordField, password);
        ClickElement(_signInBtn);
    }

    protected string UpdateUserPassword(string currentPassword, string? newPassword = null)
    {
        newPassword ??= currentPassword+currentPassword;
        EnterText(_currentPasswordField, currentPassword);
        EnterText(_newPasswordField, newPassword);
        EnterText(_confirmNewPasswordField, newPassword);
        ClickElement(_signInBtn);
        return newPassword;
    }
}