namespace UI.PageModels.Dtos;

public class ScreeningParticipantDto
{
    public string DisplayName { get; set; }
    public List<string> DisplayNamesToScreenFrom { get; set; }
    
    public ScreeningParticipantDto(string displayName, List<string> displayNamesToScreenFrom)
    {
        DisplayName = displayName;
        DisplayNamesToScreenFrom = displayNamesToScreenFrom;
    }
}