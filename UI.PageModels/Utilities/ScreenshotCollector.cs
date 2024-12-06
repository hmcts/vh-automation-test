using AventStack.ExtentReports;

namespace UI.PageModels.Utilities;

public record ScreenshotDto(string ImageBase64Encoded, string Page, string Action, Status Status = Status.Pass);

public class ScreenshotCollector
{
    private static ScreenshotCollector? _instance;
    public static ScreenshotCollector Instance()
    {
        return _instance ??= new ScreenshotCollector();
    }
    
    private readonly Dictionary<string, List<ScreenshotDto>> _screenshots = new();
    
    public void AddImage(string testKey, string imageBase64Encoded, string page,  string action, Status status = Status.Pass)
    {
        if (!_screenshots.TryGetValue(testKey, out var value))
        {
            value = [];
            _screenshots.Add(testKey, value);
        }

        var entry = new ScreenshotDto(imageBase64Encoded, page, action, status);
        value.Add(entry);
    }
    
    public List<ScreenshotDto> GetImages(string testName)
    {
        return _screenshots[testName];
    }
}