namespace UI.PageModels.Dtos;

/// <summary>
///     This represents the required information when booking a hearing with an existing participant
/// </summary>
public class BookingParticipantDto
{
    public string Username { get; set; }
    public string ContactEmail { get; set; }
    public string Phone {get; set;}
    public string DisplayName { get; set; }
    public GenericTestRole Role { get; set; }
    public string Representing { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Organisation { get; set; }
    public InterpreterLanguageDto? InterpreterLanguage { get; set; }
    
    public string FullName => $"{Title} {FirstName} {LastName}";

    public static BookingParticipantDto Individual(GenericTestParty party, GenericTestRole role,
        string contactEmail, string username, string displayName, string title, string firstName, string lastName)
    {
        return new BookingParticipantDto
        {
            Role = role,
            Username = username,
            ContactEmail = contactEmail,
            DisplayName = displayName,
            Title = title,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public static BookingParticipantDto Representative(GenericTestParty party, GenericTestRole role,
        string contactEmail, string username, string displayName, string title, string firstName, string lastName,
        string representing)
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