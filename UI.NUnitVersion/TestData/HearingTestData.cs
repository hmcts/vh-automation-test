namespace UI.NUnitVersion.TestData;

public static class HearingTestData
{
    public static string StaffMemberUsername = "auto_aw.staffmember_01@hearings.reform.hmcts.net";
    public static string VhOfficerUsername = "auto_aw.videohearingsofficer_07@hearings.reform.hmcts.net";
    /// <summary>
    /// Create a hearing with only a judge
    /// </summary>
    /// <param name="remote"></param>
    /// <param name="scheduledDateTime">a hearing with a judge and zero participants</param>
    /// <returns></returns>
    public static BookingDto CreateHearingDtoWithOnlyAJudge(bool remote = false, DateTime? scheduledDateTime = null)
    {
        var date = DateUtil.GetNow(remote);
        var hearingDateTime = scheduledDateTime ?? date.AddMinutes(5);
        var bookingDto = new BookingDto
        {
            CaseName = $"BookAHearing Automation Test {date:M-d-yy-H-mm-ss} {Guid.NewGuid():N}",
            CaseNumber = $"Automation Test Hearing {Guid.NewGuid():N}",
            CaseType = "Civil",
            HearingType = "Enforcement Hearing",
            ScheduledDateTime = hearingDateTime,
            DurationHour = 1,
            DurationMinute = 30,
            VenueName = "Birmingham Civil and Family Justice Centre",
            RoomName = "Room 1",
            Judge = new BookingJudgeDto(
                "aaron.becker.jp@ejudiciary.net",
                "Auto Judge",
                "")
            ,
            AudioRecording = false,
            OtherInformation = "This is a test hearing"
        };
        return bookingDto;
    }
    /// <summary>
    ///     Create a ejudge to Assign a presiding, email and Display name
    ///     Create a hearing with 4 participants, 2 claimants and 2 defendants
    /// </summary>
    /// <returns>a hearing with 4 participants, 2 claimants and 2 defendants</returns>
    public static BookingDto AssignaPresidingJudgeDto(bool remote = false, DateTime? scheduledDateTime = null)
    {
        var date = DateUtil.GetNow(remote);
        var hearingDateTime = scheduledDateTime ?? date.AddMinutes(5);
        var bookingDto = new BookingDto
            
        {
            CaseName = $"BookAHearing Automation Test {date:M-d-yy-H-mm-ss} {Guid.NewGuid():N}",
            CaseNumber = $"Automation Test Hearing {Guid.NewGuid():N}",
            CaseType = "Civil",
            HearingType = "Enforcement Hearing",
            ScheduledDateTime = hearingDateTime,
            DurationHour = 1,
            DurationMinute = 30,
            VenueName = "Birmingham Civil and Family Justice Centre",
            RoomName = "Room 1",
            Judge = new BookingJudgeDto(
                "aaron.becker.jp@ejudiciary.net",
                "Auto eJudge",
                ""),
            Participants = KnownParticipantsForTesting(),
            AudioRecording = false,
            OtherInformation = "This is a test hearing"
        };
        return bookingDto;
    }

    
    
    /// <summary>
    ///     Create a hearing with 4 participants, 2 claimants and 2 defendants
    /// </summary>
    /// <returns>a hearing with 4 participants, 2 claimants and 2 defendants</returns>
    public static BookingDto CreateHearingDto(bool remote = false, DateTime? scheduledDateTime = null)
    {
        var date = DateUtil.GetNow(remote);
        var hearingDateTime = scheduledDateTime ?? date.AddMinutes(5);
        var bookingDto = new BookingDto
        {
            CaseName = $"BookAHearing Automation Test {date:M-d-yy-H-mm-ss} {Guid.NewGuid():N}",
            CaseNumber = $"Automation Test Hearing {Guid.NewGuid():N}",
            CaseType = "Civil",
            HearingType = "Enforcement Hearing",
            ScheduledDateTime = hearingDateTime,
            DurationHour = 1,
            DurationMinute = 30,
            VenueName = "Birmingham Civil and Family Justice Centre",
            RoomName = "Room 1",
            Judge = new BookingJudgeDto(
                "auto_aw.judge_02@hearings.reform.hmcts.net",
                "Auto Judge",
                ""),
            Participants = KnownParticipantsForTesting(),
            AudioRecording = false,
            OtherInformation = "This is a test hearing"
        };
        return bookingDto;
    }

    /// <summary>
    /// 2 claimants (LIP and REP) and 2 defendants (LIP and REP)
    /// </summary>
    /// <returns></returns>
    public static List<BookingExistingParticipantDto> KnownParticipantsForTesting()
    {
        return new List<BookingExistingParticipantDto>
        {
            BookingExistingParticipantDto.Individual(GenericTestParty.Claimant, GenericTestRole.Appellant,
                "auto_vw.individual_60@hmcts.net", "auto_vw.individual_60@hearings.reform.hmcts.net", "Auto 1",
                "Mr", "Automation_Arnold", "Automation_Koelpin"),
            BookingExistingParticipantDto.Representative(GenericTestParty.Claimant, GenericTestRole.Representative,
                "auto_vw.representative_139@hmcts.net", "auto_vw.representative_139@hearings.reform.hmcts.net",
                "Auto 2", "Mr", "Auto_VW", "Representative_139", "Auto 1"),
            BookingExistingParticipantDto.Individual(GenericTestParty.Defendant, GenericTestRole.Appellant,
                "auto_vw.individual_137@hmcts.net", "auto_vw.individual_137@hearings.reform.hmcts.net", "Auto 3",
                "Mr", "Auto_VW", "Individual_137"),
            BookingExistingParticipantDto.Representative(GenericTestParty.Defendant, GenericTestRole.Representative,
                "auto_vw.representative_157@hmcts.net", "auto_vw.representative_157@hearings.reform.hmcts.net",
                "Auto 4", "Mr", "Automation_Torrance", "Automation_Moen", "Auto 3")
        };
    }
    
    /// <summary>
    ///     Create a hearing with 4 participants, 2 claimants, 2 defendants and 2 Video Access Points (one for each party)
    /// </summary>
    /// <returns>hearing with 4 participants, 2 claimants, 2 defendants and 2 Video Access Points (one for each party)</returns>
    public static BookingDto CreateHearingDtoWithEndpoints(bool remote = false, DateTime? scheduledDateTime = null)
    {
        var bookingDto = CreateHearingDto(remote, scheduledDateTime);
        bookingDto.VideoAccessPoints = new List<VideoAccessPointsDto>
        {
            new("Claimant VAP", "Auto 2"),
            new("Defendant VAP", "Auto 4")
        };
        return bookingDto;
    }

    public static BookNewHearingRequest CreateNewRequestDtoWithOnlyAJudge(bool remote = false,
        DateTime? scheduledDateTime = null)
    {
        var date = DateUtil.GetNow(remote);
        var hearingDateTime = scheduledDateTime ?? date.AddMinutes(5);
        
        var bookingDto = CreateHearingDtoWithOnlyAJudge(scheduledDateTime: hearingDateTime);
        var request = new BookNewHearingRequest()
        {
            Cases = new List<CaseRequest>()
            {
                {
                    new()
                    {
                        Name = bookingDto.CaseName,
                        Number = bookingDto.CaseNumber,
                        IsLeadCase = true
                    }
                }
            },
            ScheduledDateTime = bookingDto.ScheduledDateTime,
            ScheduledDuration = bookingDto.DurationHour = 90,
            HearingRoomName = bookingDto.RoomName,
            CreatedBy = "automated test framework",
            HearingTypeName = bookingDto.HearingType,
            CaseTypeName = bookingDto.CaseType,
            OtherInformation = bookingDto.OtherInformation,
            AudioRecordingRequired = bookingDto.AudioRecording,
            HearingVenueName = bookingDto.VenueName,
            Participants = new List<ParticipantRequest>()
            {
                new()
                {
                    FirstName = "Auto_AW",
                    LastName = "Judge_02",
                    DisplayName = bookingDto.Judge.DisplayName,
                    Username = bookingDto.Judge.Username,
                    CaseRoleName = "Judge",
                    HearingRoleName = "Judge",
                    TelephoneNumber = null,
                    ContactEmail = bookingDto.Judge.Username
                }
            }
        };
        return request;
    }
}