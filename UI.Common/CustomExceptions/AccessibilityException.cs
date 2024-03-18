namespace UI.Common.CustomExceptions;

public class AccessibilityException : AggregateException
{
    private const string DefaultMessage = "Accessibility check failed. Please check the report for more details.";

    public AccessibilityException(string message) 
        : base(String.IsNullOrEmpty(message) ? DefaultMessage : message)
    {
    }

    public AccessibilityException(string message, IEnumerable<AccessibilityException>exceptions) 
        : base(String.IsNullOrEmpty(message) ? DefaultMessage : message, exceptions)
    {
    }
}