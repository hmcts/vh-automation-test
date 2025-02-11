namespace UI.PageModels.Dtos;

public class BookingJudiciaryParticipantDto(string personalCode, string username, string displayName, string phone)
{
    public string Username { get; set; } = username;
    public string DisplayName { get; set; } = displayName;
    public string Phone { get; set; } = phone;
    public InterpreterLanguageDto? InterpreterLanguage { get; set; }
    public string PersonalCode { get; set; } = personalCode;
}
