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
    }
    
    /// <summary>
    /// Validates the screening details match those in the dto
    /// </summary>
    /// <param name="bookingDto"></param>
    public void ValidateScreeningDetails(BookingDto bookingDto)
    {
        var expectedScreeningCount = bookingDto.ScreeningParticipants.Count;
        if (expectedScreeningCount == 0) return;
        
        var screeningElements = Driver.FindElements(By.XPath("//div[contains(@class, 'participant-row__screening')]"));
        CompareNumbers(screeningElements.Count, expectedScreeningCount);
        foreach (var screeningElement in screeningElements)
        {
            var text = screeningElement.Text;
            CompareText(text, "Screening enabled");
        }
    }
    
    protected static void CompareText(string text, string expectedText)
    {
        if (!text.Equals(expectedText.Trim(), StringComparison.InvariantCultureIgnoreCase))
        {
            throw new InvalidOperationException($"Expected text: {expectedText} but was {text}");
        }
    }
    
    private static void CompareNumbers(int number, int expectedNumber)
    {
        if (number != expectedNumber)
        {
            throw new InvalidOperationException($"Expected number: {expectedNumber} but was {number}");
        }
    }
}