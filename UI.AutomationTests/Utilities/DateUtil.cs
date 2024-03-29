namespace UI.AutomationTests.Utilities;

public static class DateUtil
{
    public static DateTime GetNow(bool remote)
    {
        return remote ? DateTime.UtcNow : DateTime.Now;
    }
}