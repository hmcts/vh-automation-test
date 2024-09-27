using UI.Common.Utilities;

namespace UI.PageModels.Pages.Admin.Booking;

public class VideoAccessPointsPage : VhAdminWebPage
{
    private readonly By _displayNameTextField = By.Id("displayName");
    private readonly By _defenceAdvocateSelector = By.Id("representative");
    private readonly By _saveOrUpdateButton = By.Id("confirmEndpointBtn");
    private readonly By _nextButton = By.Id("nextButton");
    private readonly By _interpreterRequired = By.Name("interpreter-required");
    private readonly By _spokenLanguageDropdown = By.Id("verbal-language");
    private readonly By _signLanguageDropdown = By.Id("sign-language");

    public VideoAccessPointsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeVisible(_displayNameTextField);
    }

    /// <summary>
    /// Add a list of video access points to a hearing
    /// </summary>
    /// <param name="videoAccessPoints"></param>
    public void AddVideoAccessPoints(List<VideoAccessPointsDto> videoAccessPoints)
    {
        foreach (var vap in videoAccessPoints)
        {
            AddVideoEndpoint(vap.DisplayName,vap.DefenceAdvocateDisplayName, vap.InterpreterLanguage);
        }
    }

    /// <summary>
    /// Add a video access point to a hearing
    /// </summary>
    /// <param name="displayName">The display name for the access point</param>
    /// <param name="defenceAdvocateDisplayName">The defence advocate to link to, if provided</param>
    /// <param name="interpreterLanguage">The interpreter language required, if provided</param>
    public void AddVideoEndpoint(string displayName, string defenceAdvocateDisplayName, InterpreterLanguageDto? interpreterLanguage = null)
    {
        EnterText(_displayNameTextField, displayName);

        if (!string.IsNullOrWhiteSpace(defenceAdvocateDisplayName))
        {
            SelectDropDownByText(_defenceAdvocateSelector, defenceAdvocateDisplayName);
        }
        
        if (interpreterLanguage != null)
        {
            var interpreterRequiredCheckbox= FindElement(_interpreterRequired);
            if (interpreterRequiredCheckbox is { Selected: false })
            {
                ClickElement(_interpreterRequired, waitToBeClickable: false);
            }
            SelectInterpreterLanguage(interpreterLanguage);
        }
        
        ClickElement(_saveOrUpdateButton);
    }
    
    private void SelectInterpreterLanguage(InterpreterLanguageDto interpreterLanguage)
    {
        switch (interpreterLanguage.Type)
        {
            case InterpreterType.Sign:
                WaitForDropdownListToPopulate(_signLanguageDropdown, 0);
                SelectDropDownByText(_signLanguageDropdown, interpreterLanguage.Description);
                break;
            case InterpreterType.Verbal:
                WaitForDropdownListToPopulate(_spokenLanguageDropdown, 0);
                SelectDropDownByText(_spokenLanguageDropdown, interpreterLanguage.Description);
                break;
            default:
                throw new InvalidOperationException("Unknown interpreter language type: " + interpreterLanguage.Type);
        }
    }

    public void RemoveVideoAccessPoint(int index)
    {
        var removeEndpointXPath = $"(//a[@class='vhlink'][normalize-space()='Remove'])[{index + 1}]";
        ClickElement(By.XPath(removeEndpointXPath));
    }

    public void UpdateVideoAccessPoint(int index, string defenceAdvocateDisplayName, InterpreterLanguageDto? interpreterLanguage = null)
    {
        var editEndpointXPath = $"(//a[@class='vhlink'][normalize-space()='Edit'])[{index + 1}]";
        ClickElement(By.XPath(editEndpointXPath));
        
        SelectDropDownByText(_defenceAdvocateSelector, defenceAdvocateDisplayName);

        if (interpreterLanguage != null)
        {
            SelectInterpreterLanguage(interpreterLanguage);
        }
        
        ClickElement(_saveOrUpdateButton);
    }
    
    /// <summary>
    /// Click next and go to the other information page (only when in create mode)
    /// </summary>
    /// <returns></returns>
    public OtherInfoPage GoToOtherInformationPage()
    {
        ClickElement(_nextButton);
        
        var specialMeasuresEnabled = FeatureToggle.Instance().SpecialMeasuresEnabled();
        return specialMeasuresEnabled ? 
            new SpecialMeasuresPage(Driver, DefaultWaitTime).GoToOtherInformationPage() : new OtherInfoPage(Driver, DefaultWaitTime);
    }

    /// <summary>
    /// When in edit mode, the next button directs a user to the summary page
    /// </summary>
    /// <returns></returns>
    public SummaryPage GoToSummaryPage()
    {
        ClickElement(_nextButton);
        
        var specialMeasuresEnabled = FeatureToggle.Instance().SpecialMeasuresEnabled();
        return specialMeasuresEnabled ? 
            new SpecialMeasuresPage(Driver, DefaultWaitTime).GoToSummaryPage() : new SummaryPage(Driver, DefaultWaitTime);
    }
}