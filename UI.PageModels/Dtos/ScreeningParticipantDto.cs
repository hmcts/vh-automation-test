namespace UI.PageModels.Dtos;

public class ScreeningParticipantDto(string displayName, ScreeningDto screening)
{
    public string DisplayName { get; } = displayName;
    public ScreeningDto Screening { get; } = screening;
}