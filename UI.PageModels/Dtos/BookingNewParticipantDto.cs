namespace UI.PageModels.Dtos;

/// <summary>
///     This represents the required information when booking a hearing with a new participant
/// </summary>
public class BookingNewParticipantDto
{
    public BookingNewParticipantDto(BookingExistingParticipantDto existing)
    {
        Title = existing.Title;
        FirstName = existing.FirstName;
        LastName = existing.LastName;
        ContactEmail = existing.ContactEmail;
        Role = existing.Role;
    }

    public GenericTestRole Role { get; set; }

    public string ContactEmail { get; set; }
    public string Title { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Organisation { get; set; }
    public string Telephone { get; set; }
    public string DisplayName { get; set; }
}