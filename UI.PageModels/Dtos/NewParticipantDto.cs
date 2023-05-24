namespace UI.PageModels.Dtos;

public class NewParticipantDto
{
    public NewParticipantDto(string title, string firstName, string lastName, string organisation, string telephone)
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