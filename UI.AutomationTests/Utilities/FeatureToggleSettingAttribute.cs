namespace UI.AutomationTests.Utilities;

/// <summary>
/// Check if a feature toggle is enabled or disabled before running a test
/// </summary>
public class FeatureToggleSettingAttribute : TestActionAttribute
{
    private readonly string _key;
    private readonly bool _expected;

    public FeatureToggleSettingAttribute(string key, bool expected)
    {
        _key = key;
        _expected = expected;
    }

    public override void BeforeTest(ITest test)
    {
        base.BeforeTest(test);

        bool actualValue;
        try
        {
            actualValue = FeatureToggle.Instance().GetBoolValueWithKey(_key);
        }
        catch (InvalidOperationException)
        {
            Assert.Fail("Test ignored because LaunchDarkly client not initialized");
            return;
        }

        if (actualValue != _expected)
        {
            Assert.Ignore($"Test ignored because feature toggle {_key} does not match the expected value {_expected}");
        }
    }
}