namespace UI.PageModels.Dtos;

/// <summary>
///     This represents a hearing that is to be booked
/// </summary>
public class BookingDto
{
    public BookingDto()
    {
        Participants = new List<BookingParticipantDto>();
        VideoAccessPoints = new List<VideoAccessPointsDto>();
    }
    public required string CaseName { get; set; }
    public required string CaseNumber { get; set; }
    public required string CaseType { get; set; }
    public required string ServiceId { get; set; }
    public DateTime ScheduledDateTime { get; set; }
    
    public DateTime EndDateTime { get; set; }
    public int DurationHour { get; set; }
    public int DurationMinute { get; set; }
    public required string VenueName { get; set; }
    public required string VenueCode { get; set; }
    public required string RoomName { get; set; }
    public bool AudioRecording { get; set; } = true;
    public required string OtherInformation { get; set; }
    public List<BookingParticipantDto> Participants { get; set; }
    public List<BookingParticipantDto> NewParticipants { get; set; } = [];
    public List<VideoAccessPointsDto> VideoAccessPoints { get; set; }
    public required BookingJudiciaryParticipantDto Judge { get; set; }
    public List<BookingJudiciaryParticipantDto> PanelMembers { get; set; } = [];

    public List<ScreeningParticipantDto> ScreeningParticipants
    {
        get
        {
            return Participants
                .Where(x => x.Screening != null)
                .Select(x => new ScreeningParticipantDto(x.DisplayName, x.Screening))
                .Concat(VideoAccessPoints
                    .Where(x => x.Screening != null)
                    .Select(x =>  new ScreeningParticipantDto(x.DisplayName, x.Screening)))
                .ToList();
        }
    }
}