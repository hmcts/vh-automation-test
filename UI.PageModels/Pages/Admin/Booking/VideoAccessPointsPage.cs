using OpenQA.Selenium;
using UI.PageModels.Dtos;

namespace UI.PageModels.Pages.Admin.Booking;

public class VideoAccessPointsPage : VhAdminWebPage
{
    private readonly By _addAnotherBtn = By.Id("addEndpoint");
    private readonly By _nextButton = By.Id("nextButton");

    public VideoAccessPointsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeVisible(_addAnotherBtn);
    }
    
    private static By RemoveDisplayName(int number) => By.Id($"removeDisplayName{number}");

    /// <summary>
    /// Add a list of video access points to a hearing
    /// </summary>
    /// <param name="videoAccessPoints"></param>
    public void AddVideoAccessPoints(List<VideoAccessPointsDto> videoAccessPoints)
    {
        for (var i = 0; i < videoAccessPoints.Count; i++)
        {
            var vap = videoAccessPoints[i];
            AddVideoEndpoint(vap.DisplayName, vap.DefenceAdvocateDisplayName, i);
            if (i < videoAccessPoints.Count - 1)
            {
                ClickElement(_addAnotherBtn);
            }
        }
    }
    
    /// <summary>
    /// Add a video access point to a hearing
    /// </summary>
    /// <param name="displayName">The display name for the access point</param>
    /// <param name="defenceAdvocateDisplayName">The defence advocate to link to, if provided</param>
    /// <param name="currentCount">The current number of endpoints already added to a hearing, excluding the one being added now</param>
    public void AddVideoEndpoint(string displayName, string defenceAdvocateDisplayName, int currentCount = 0)
    {
        // enter the video endpoints display name, which is index based
        var displayNameTextField = By.Id($"displayName{currentCount}");
        EnterText(displayNameTextField, displayName);

        if (!string.IsNullOrWhiteSpace(defenceAdvocateDisplayName))
        {
            // defence advocate selector
            var defenceAdvocateSelector = By.Id($"defenceAdvocate{currentCount}");
            SelectDropDownByText(defenceAdvocateSelector, defenceAdvocateDisplayName);
        }
    }
    
    /// <summary>
    /// Click next and go to the other information page (only when in create mode)
    /// </summary>
    /// <returns></returns>
    public OtherInfoPage GoToOtherInformationPage()
    {
        ClickElement(_nextButton);
        return new OtherInfoPage(Driver, DefaultWaitTime);
    }
    
    
    /// <summary>
    /// When in edit mode, the next button directs a user to the summary page
    /// </summary>
    /// <returns></returns>
    public SummaryPage GoToSummaryPage()
    {
        ClickElement(_nextButton);
        return new SummaryPage(Driver, DefaultWaitTime);
    }
}