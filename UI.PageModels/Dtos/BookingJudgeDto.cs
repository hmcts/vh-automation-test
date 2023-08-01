namespace UI.PageModels.Dtos;

public class BookingJudgeDto
{
    public BookingJudgeDto(string username, string displayName, string phone)
    {
        Username = username;
        DisplayName = displayName;
        Phone = phone;
    }

    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Phone { get; set; }
}