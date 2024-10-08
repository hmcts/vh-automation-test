using Selenium.Axe;

namespace UI.PageModels.Utilities;

public class AccessibilityResult(AxeResult result, IWebDriver driver)
{
    public AxeResult Result { get; private set; } = result;
    public IWebDriver Driver { get; private set; } = driver;
}