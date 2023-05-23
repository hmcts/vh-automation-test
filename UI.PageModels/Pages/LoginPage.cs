using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TestFramework;

namespace UI.PageModels.Pages
{
	public class LoginPage : VhPage
	{

		private readonly By _usernameTextfield = By.Id("i0116");
		private readonly By _passwordField = By.Id("i0118");
		private readonly By _nextBtn = By.Id("idSIButton9");
		private readonly By _signInBtn = By.Id("idSIButton9"); // also the next button
		// private readonly By LoginHeader = By.Id("loginHeader");
		
		// private readonly string SignInTitle = "Sign in to your account";
		// private readonly string SignOutTitle = "Sign out";
		// private readonly By ReSignInButton(string username) => By.XPath($"//div[contains(text(), '{username}')]");
		// private readonly By CurrentPassword = By.Id("currentPassword");
		// private readonly By NewPassword = By.Id("newPassword");
		// private readonly By ConfirmNewPassword = By.Id("confirmNewPassword");
		// private readonly By SignInButtonAfterPasswordChange = By.Id("idSIButton9");
		// private readonly By BackButton = By.Id("idBtn_Back");
		// private readonly By AccountTypeJudge = By.Id("ejud");
		// private readonly By AccountTypeParticipant = By.Id("vhaad");

		public LoginPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
		{
			WaitForElementToBeClickable(_nextBtn);
		}

		public DashboardPage Login(string username, string password)
		{
			EnterText(_usernameTextfield, username);
			ClickElement(_nextBtn);
			
			EnterText(_passwordField, password);
			ClickElement(_signInBtn);
			return new DashboardPage(Driver, DefaultWaitTime);
		}
	}
}
