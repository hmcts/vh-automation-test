namespace UI.PageModels.Dtos;

/// <summary>
///     This represents the required information when booking a hearing with an existing participant
/// </summary>
public class BookingParticipantDto
{
    public required string Username { get; set; }
    public required string ContactEmail { get; set; }
    public string Phone { get; set; } = string.Empty;
    public required string DisplayName { get; set; }
    public GenericTestRole Role { get; set; }
    public string? Representing { get; set; }
    public required string Title { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Organisation { get; set; }
    public InterpreterLanguageDto? InterpreterLanguage { get; set; }
    public string? VideoFileName { get; set; }
    
    public string FullName => $"{Title} {FirstName} {LastName}";
    public ScreeningDto? Screening { get; set; }

    public static BookingParticipantDto Individual(GenericTestRole role,
        string contactEmail, string username, string displayName, string title, string firstName, string lastName, string? videoFileName)
    {
        return new BookingParticipantDto
        {
            Role = role,
            Username = username,
            ContactEmail = contactEmail,
            DisplayName = displayName,
            Title = title,
            FirstName = firstName,
            LastName = lastName,
            VideoFileName = videoFileName
        };
    }

    public static BookingParticipantDto Representative(GenericTestRole role,
        string contactEmail, string username, string displayName, string title, string firstName, string lastName,
        string representing, string videoFileName)
    {
        return new BookingParticipantDto
        {
            Role = role,
            Username = username,
            ContactEmail = contactEmail,
            DisplayName = displayName,
            Representing = representing,
            Title = title,
            FirstName = firstName,
            LastName = lastName
        };
    }
}