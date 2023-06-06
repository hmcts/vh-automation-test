namespace UI.PageModels.Dtos;

/// <summary>
///     This represents the required information when booking a hearing with an existing participant
/// </summary>
public class BookingExistingParticipantDto
{
    public GenericTestParty Party { get; set; }
    public GenericTestRole Role { get; set; }
    public string ContactEmail { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Representing { get; set; }

    public static BookingExistingParticipantDto Individual(GenericTestParty party, GenericTestRole role,
        string contactEmail, string username, string displayName)
    {
        return new BookingExistingParticipantDto
        {
            Party = party,
            Role = role,
            Username = username,
            ContactEmail = contactEmail,
            DisplayName = displayName
        };
    }

    public static BookingExistingParticipantDto Representative(GenericTestParty party, GenericTestRole role,
        string contactEmail, string username, string displayName, string representing)
    {
        return new BookingExistingParticipantDto
        {
            Party = party,
            Role = role,
            Username = username,
            ContactEmail = contactEmail,
            DisplayName = displayName,
            Representing = representing
        };
    }
}