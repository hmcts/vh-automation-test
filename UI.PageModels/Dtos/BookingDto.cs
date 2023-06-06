namespace UI.PageModels.Dtos;

/// <summary>
///     This represents a hearing that is to be booked
/// </summary>
public class BookingDto
{
    public string CaseName { get; set; }
    public string CaseNumber { get; set; }
    public string CaseType { get; set; }
    public string HearingType { get; set; }
    public DateTime ScheduledDateTime { get; set; }
    public int DurationHour { get; set; }
    public int DurationMinute { get; set; }
    public string VenueName { get; set; }
    public string RoomName { get; set; }
    public List<BookingExistingParticipantDto> Participants { get; set; }
    public BookingJudgeDto Judge { get; set; }
}