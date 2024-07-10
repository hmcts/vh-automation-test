using LaunchDarkly.Sdk;

namespace UI.AutomationTests.TestData;

public static class HearingTestData
{
    public static string StaffMemberUsername = "auto_aw.staffmember_01@hearings.reform.hmcts.net";
    public static string VhOfficerUsername = "auto_aw.videohearingsofficer_07@hearings.reform.hmcts.net";
    public static string AltJudge = "auto_aw.judge_01@hearings.reform.hmcts.net";
    public static string Judge = "auto_aw.judge_02@hearings.reform.hmcts.net";
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
            Judge = new BookingJudgeDto(Judge, "Auto Judge", ""),
            AudioRecording = false,
            OtherInformation = "This is a test hearing"
        };
        return bookingDto;
    }
    
    /// <summary>
    ///     Create a hearing with 4 participants, 2 claimants and 2 defendants
    /// </summary>
    /// <returns>a hearing with 4 participants, 2 claimants and 2 defendants</returns>
    public static BookingDto CreateHearingDto(string judgeUsername, bool remote = false, DateTime? scheduledDateTime = null, bool includeInterpreter = false)
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
                judgeUsername,
                "Auto Judge",
                ""),
            Participants = KnownParticipantsForTesting(includeInterpreter: includeInterpreter),
            AudioRecording = false,
            OtherInformation = "This is a test hearing"
        };
        return bookingDto;
    }

    public static BookingDto CreateHearingDtoWithInterpreterLanguages(string judgeUsername, DateTime scheduledDateTime)
    {
        const string interpreterLanguageDescription = "Spanish";
        var bookingDto = CreateHearingDtoWithEndpoints(judgeUsername, scheduledDateTime: scheduledDateTime, includeInterpreter: true);
        bookingDto.Judge.InterpreterLanguageDescription = interpreterLanguageDescription;
        foreach (var participant in bookingDto.Participants)
        {
            participant.InterpreterLanguageDescription = interpreterLanguageDescription;
        }
        foreach (var endpoint in bookingDto.VideoAccessPoints)
        {
            endpoint.InterpreterLanguageDescription = interpreterLanguageDescription;
        }
        return bookingDto;
    }

    /// <summary>
    /// 2 claimants (LIP and REP) and 2 defendants (LIP and REP)
    /// </summary>
    /// <returns></returns>
    public static List<BookingParticipantDto> KnownParticipantsForTesting(bool includeInterpreter = false)
    {
        var participants = new List<BookingParticipantDto>
        {
            BookingParticipantDto.Individual(GenericTestParty.Claimant, GenericTestRole.Witness,
                "auto_vw.individual_60@hmcts.net", "auto_vw.individual_60@hearings.reform.hmcts.net", "Auto 1",
                "Mr", "Automation_Arnold", "Automation_Koelpin"),
            BookingParticipantDto.Representative(GenericTestParty.Claimant, GenericTestRole.Representative,
                "auto_vw.representative_139@hmcts.net", "auto_vw.representative_139@hearings.reform.hmcts.net",
                "Auto 2", "Mr", "Auto_VW", "Representative_139", "Auto 1"),
            BookingParticipantDto.Individual(GenericTestParty.Defendant, GenericTestRole.Witness,
                "auto_vw.individual_137@hmcts.net", "auto_vw.individual_137@hearings.reform.hmcts.net", "Auto 3",
                "Mr", "Auto_VW", "Individual_137"),
            BookingParticipantDto.Representative(GenericTestParty.Defendant, GenericTestRole.Representative,
                "auto_vw.representative_157@hmcts.net", "auto_vw.representative_157@hearings.reform.hmcts.net",
                "Auto 4", "Mr", "Automation_Torrance", "Automation_Moen", "Auto 3")
        };

        if (includeInterpreter)
        {
            participants.Add(BookingParticipantDto.Individual(GenericTestParty.Applicant, GenericTestRole.Interpreter,
                "Automation_Claimant_Interpreter_1@hmcts.net", "automation_claimant_interpreter_1@hearings.reform.hmcts.net", "Auto 5",
                "Mr", "Auto_VW", "Interpreter_1"));
        }

        return participants;
    }
    
    /// <summary>
    ///     Create a hearing with 4 participants, 2 claimants, 2 defendants and 2 Video Access Points (one for each party)
    /// </summary>
    /// <returns>hearing with 4 participants, 2 claimants, 2 defendants and 2 Video Access Points (one for each party)</returns>
    public static BookingDto CreateHearingDtoWithEndpoints(string judgeUsername, bool remote = false, DateTime? scheduledDateTime = null, bool includeInterpreter = false)
    {
        var bookingDto = CreateHearingDto(judgeUsername: judgeUsername, remote, scheduledDateTime, includeInterpreter: includeInterpreter);
        bookingDto.VideoAccessPoints = new List<VideoAccessPointsDto>
        {
            new("Claimant VAP", "Auto 2"),
            new("Defendant VAP", "Auto 4")
        };
        if (includeInterpreter)
        {
            bookingDto.AudioRecording = true;
        }
        return bookingDto;
    }

    public static VideoAccessPointsDto CreateNewEndpointDto()
    {
        var timestamp = AddTimeStamp();
        var displayName = $"VAP {timestamp}";
        const string defenceAdvocateDisplayName = "";

        return new VideoAccessPointsDto(displayName, defenceAdvocateDisplayName);
    }
    
    public static BookingParticipantDto CreateNewParticipantDto()
    {
        var timeStamp = AddTimeStamp();
        var user = BookingParticipantDto.Individual(GenericTestParty.Claimant, GenericTestRole.Witness,
            $"New_User{timeStamp}@hmcts.net",
            $"New.User{timeStamp}@hearings.reform.hmcts.net", "NewCreatedUser",
            "Mr", $"New", $"User{timeStamp}");
        user.Organisation = "HMCTS";
        user.Phone = "0123456789";
        return user;
    }
    
    public static string AddTimeStamp() => DateTime.Now.ToString("yyyyMMddHHmmssfff");

    public static BookingDto CreateMultiDayDto(int numberOfDays, DateTime scheduledDateTime  )
    {
        var bookingDto = CreateHearingDto(Judge, false, scheduledDateTime );
        bookingDto.EndDateTime = scheduledDateTime.AddDays(numberOfDays);
        return bookingDto;
    }

    public static BookingDto CreateMultiDayDtoWithEndpoints(int numberOfDays, DateTime scheduledDateTime)
    {
        var bookingDto = CreateHearingDtoWithEndpoints(Judge, false, scheduledDateTime );
        bookingDto.EndDateTime = scheduledDateTime.AddDays(numberOfDays);
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