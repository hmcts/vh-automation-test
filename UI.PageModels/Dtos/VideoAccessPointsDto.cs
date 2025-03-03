namespace UI.PageModels.Dtos;

public class VideoAccessPointsDto
{
    
    public string DisplayName { get; set; }
    public string DefenceAdvocateDisplayName { get; set; }
    public InterpreterLanguageDto? InterpreterLanguage { get; set; }
    public ScreeningDto? Screening { get; set; }
    
    public VideoAccessPointsDto(string displayName, string defenceAdvocateDisplayName)
    {
        DisplayName = displayName;
        DefenceAdvocateDisplayName = defenceAdvocateDisplayName;
    }
}