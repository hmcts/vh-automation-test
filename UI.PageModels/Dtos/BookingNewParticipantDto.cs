namespace UI.PageModels.Dtos;

/// <summary>
///     This represents the required information when booking a hearing with a new participant
/// </summary>
public class BookingNewParticipantDto
{
    public BookingNewParticipantDto(string title, string firstName, string lastName, string organisation,
        string telephone)
    {
        Title = title;
        FirstName = firstName;
        LastName = lastName;
        Organisation = organisation;
        Telephone = telephone;
    }

    public string Title { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Organisation { get; }
    public string Telephone { get; }
}