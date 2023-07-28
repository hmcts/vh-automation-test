namespace UI.PageModels.Dtos;

public class VideoAccessPointsDto
{
    
    public string DisplayName { get; set; }
    public string DefenceAdvocateDisplayName { get; set; }
    
    public VideoAccessPointsDto(string displayName, string defenceAdvocateDisplayName)
    {
        DisplayName = displayName;
        DefenceAdvocateDisplayName = defenceAdvocateDisplayName;
    }
}