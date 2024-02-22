namespace UI.PageModels.Dtos;

/// <summary>
///     This represents a hearing that is to be booked
/// </summary>
public class BookingDto
{
    public BookingDto()
    {
        Participants = new List<BookingExistingParticipantDto>();
        VideoAccessPoints = new List<VideoAccessPointsDto>();
      
    }
    public string CaseName { get; set; }
    public string CaseNumber { get; set; }
    public string CaseType { get; set; }
    public string HearingType { get; set; }
    public DateTime ScheduledDateTime { get; set; }
    
    public DateTime EndDateTime { get; set; }
    public int DurationHour { get; set; }
    public int DurationMinute { get; set; }
    public string VenueName { get; set; }
    public string RoomName { get; set; }
    public bool AudioRecording { get; set; } = true;
    public string OtherInformation { get; set; }
    
    public BookingDto AddAJudgeWarning { get; set; }
    public List<BookingExistingParticipantDto> Participants { get; set; }
    public List<VideoAccessPointsDto> VideoAccessPoints { get; set; }
    public BookingJudgeDto Judge { get; set; }
    
    
    public List<BookingJudgeDto> AdditionalJudges { get; set; }
    public string NewParticipants { get; set; }
}