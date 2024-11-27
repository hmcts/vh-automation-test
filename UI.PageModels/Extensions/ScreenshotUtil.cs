using AventStack.ExtentReports;
using UI.PageModels.Pages;
using UI.PageModels.Utilities;

namespace UI.PageModels.Extensions;

public static class ScreenshotUtil
{
    /// <summary>
    /// Take a screenshot and save to the screenshot collector
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="sectionTitle"></param>
    /// <param name="imageTitle">default 'Page Load'</param>
    /// <param name="status"></param>
    public static void TakeScreenshotAndSave(this IWebDriver driver, string sectionTitle, string imageTitle,  Status status = Status.Pass)
    {
        var testName = Environment.GetEnvironmentVariable(VhPage.VHTestNameKey);
        var imageBase64Encoded = driver.TakeScreenshotAsBase64();
        var screenshotCollector = ScreenshotCollector.Instance();
        screenshotCollector.AddImage(testName!, imageBase64Encoded, sectionTitle, imageTitle, status);
    }
    
    /// <summary>
    /// Take a screenshot and return as a Base64 encoded string
    /// </summary>
    /// <param name="driver"></param>
    public static string TakeScreenshotAsBase64(this IWebDriver driver)
    {
        var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        return screenshot.AsBase64EncodedString;
    }
}