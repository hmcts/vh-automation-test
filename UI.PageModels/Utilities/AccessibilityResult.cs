using Selenium.Axe;

namespace UI.PageModels.Utilities;

public class AccessibilityResult
{
    private AccessibilityResult(AxeResult axeResult)
    {
        Result = axeResult;
    }
    
    public AxeResult Result { get; private set; }

    /// <summary>
    /// If the result has not already been captured, saves the result and creates a report
    /// </summary>
    public static void Capture(IWebDriver driver)
    {
        var axeBuilder = new AxeBuilder(driver);
        var axeResult = axeBuilder.Analyze();
        var result = new AccessibilityResult(axeResult);
        
        if (AccessibilityResultCollection.Add(result))
            AccessibilityReport.Create(result, driver);
    }
}