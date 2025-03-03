namespace UI.PageModels.Dtos;

public class ScreeningEntityDto
{
    public string DisplayName { get; set; }

    public ScreeningEntityDto(string displayName)
    {
        DisplayName = displayName;
    }
}