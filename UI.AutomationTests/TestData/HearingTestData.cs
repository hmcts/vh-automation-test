using BookingsApi.Contract.V1.Requests.Enums;
using BookingsApi.Contract.V2.Enums;
using UI.Common.Utilities;

namespace UI.AutomationTests.TestData;

public static class HearingTestData
{
    public static string StaffMemberUsername = "auto_aw.staffmember_01@hearings.reform.hmcts.net";
    public static string VhOfficerUsername = "auto_aw.videohearingsofficer_07@hearings.reform.hmcts.net";
    public static string AltJudgeUsername = "auto_aw.judge_01@hearings.reform.hmcts.net";
    public static string AltJudgePersonalCode = "VH-GENERIC-ACCOUNT-00";
    public static string JudgeUsername = "auto_aw.judge_02@hearings.reform.hmcts.net";
    public static string JudgePersonalCode = "VH-GENERIC-ACCOUNT-0";
    
    public const string ClerkVideoFileName = "clerk.y4m";
    public const string Individual01FileName = "individual01.y4m";
    public const string Individual02FileName = "individual02.y4m";
    public const string Representative01FileName = "representative01.y4m";
    public const string Representative02FileName = "representative02.y4m";

    private const string HearingVenueName = "Birmingham Civil and Family Justice Centre";
    private const string HearingVenueCode = "231596";

    private const string CaseType = "Adoption";
    private const string ServiceId = "ABA4";

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
            CaseType = CaseType,
            ServiceId = ServiceId,
            ScheduledDateTime = hearingDateTime,
            DurationHour = 1,
            DurationMinute = 30,
            VenueName = HearingVenueName,
            VenueCode = HearingVenueCode,
            RoomName = "Room 1",
            Judge = new BookingJudgeDto(JudgePersonalCode, JudgeUsername, "Auto Judge", ""),
            AudioRecording = false,
            OtherInformation = "This is a test hearing"
        };
        return bookingDto;
    }

    /// <summary>
    ///     Create a hearing with 4 participants, 2 claimants and 2 defendants
    /// </summary>
    /// <returns>a hearing with 4 participants, 2 claimants and 2 defendants</returns>
    public static BookingDto CreateHearingDto(string judgePersonalCode, string judgeUsername, bool remote = false,
        DateTime? scheduledDateTime = null, bool includeInterpreter = false)
    {
        var date = DateUtil.GetNow(remote);
        var hearingDateTime = scheduledDateTime ?? date.AddMinutes(5);
        var bookingDto = new BookingDto
        {
            CaseName = $"BookAHearing Automation Test {date:M-d-yy-H-mm-ss} {Guid.NewGuid():N}",
            CaseNumber = $"Automation Test Hearing {Guid.NewGuid():N}",
            CaseType = CaseType,
            ServiceId = ServiceId,
            ScheduledDateTime = hearingDateTime,
            DurationHour = 1,
            DurationMinute = 30,
            VenueName = "Birmingham Civil and Family Justice Centre",
            VenueCode = HearingVenueCode,
            RoomName = "Room 1",
            Judge = new BookingJudgeDto(
                judgePersonalCode,
                judgeUsername,
                "Auto Judge",
                ""),
            Participants = KnownParticipantsForTesting(includeInterpreter: includeInterpreter),
            AudioRecording = false,
            OtherInformation = "This is a test hearing"
        };
        return bookingDto;
    }

    public static BookingDto CreateHearingDtoWithInterpreterLanguages(string judgePersonalCode, string judgeUsername,
        DateTime scheduledDateTime, InterpreterLanguageDto interpreterLanguage)
    {
        var bookingDto = CreateHearingDtoWithEndpoints(judgePersonalCode, judgeUsername,
            scheduledDateTime: scheduledDateTime, includeInterpreter: true);
        bookingDto.Judge.InterpreterLanguage = interpreterLanguage;
        foreach (var participant in bookingDto.Participants)
        {
            participant.InterpreterLanguage = interpreterLanguage;
        }

        foreach (var endpoint in bookingDto.VideoAccessPoints)
        {
            endpoint.InterpreterLanguage = interpreterLanguage;
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
            BookingParticipantDto.Individual(GenericTestRole.Witness,
                "auto_vw.individual_60@hmcts.net", "auto_vw.individual_60@hearings.reform.hmcts.net", "Auto 1",
                "Mr", "Automation_Arnold", "Automation_Koelpin", Individual01FileName),
            BookingParticipantDto.Representative(GenericTestRole.Representative,
                "auto_vw.representative_139@hmcts.net", "auto_vw.representative_139@hearings.reform.hmcts.net",
                "Auto 2", "Mr", "Auto_VW", "Representative_139", "Auto 1", Representative01FileName),
            BookingParticipantDto.Individual(GenericTestRole.Witness,
                "auto_vw.individual_137@hmcts.net", "auto_vw.individual_137@hearings.reform.hmcts.net", "Auto 3",
                "Mr", "Auto_VW", "Individual_137", Individual02FileName),
            BookingParticipantDto.Representative(GenericTestRole.Representative,
                "auto_vw.representative_157@hmcts.net", "auto_vw.representative_157@hearings.reform.hmcts.net",
                "Auto 4", "Mr", "Automation_Torrance", "Automation_Moen", "Auto 3", Representative02FileName)
        };

        if (includeInterpreter)
        {
            participants.Add(BookingParticipantDto.Individual(GenericTestRole.Interpreter,
                "Automation_Claimant_Interpreter_1@hmcts.net",
                "automation_claimant_interpreter_1@hearings.reform.hmcts.net", "Auto 5",
                "Mrs", "Automation_Claimant", "Interpreter_1", null));
        }

        return participants;
    }

    /// <summary>
    ///     Create a hearing with 4 participants, 2 claimants, 2 defendants and 2 Video Access Points (one for each party)
    /// </summary>
    /// <returns>hearing with 4 participants, 2 claimants, 2 defendants and 2 Video Access Points (one for each party)</returns>
    public static BookingDto CreateHearingDtoWithEndpoints(string judgePersonalCode, string judgeUsername,
        bool remote = false, DateTime? scheduledDateTime = null, bool includeInterpreter = false)
    {
        var bookingDto = CreateHearingDto(judgePersonalCode, judgeUsername, remote, scheduledDateTime,
            includeInterpreter: includeInterpreter);
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
        var user = BookingParticipantDto.Individual(GenericTestRole.Witness,
            $"New_User{timeStamp}@hmcts.net",
            $"New.User{timeStamp}@hearings.reform.hmcts.net", "NewCreatedUser",
            "Mr", $"New", $"User{timeStamp}", null);
        user.Organisation = "HMCTS";
        user.Phone = "0123456789";
        return user;
    }

    public static string AddTimeStamp() => DateTime.Now.ToString("yyyyMMddHHmmssfff");

    public static BookingDto CreateMultiDayDto(int numberOfDays, DateTime scheduledDateTime)
    {
        var bookingDto = CreateHearingDto(JudgePersonalCode, JudgeUsername, false, scheduledDateTime);
        bookingDto.EndDateTime = scheduledDateTime.AddDays(numberOfDays);
        return bookingDto;
    }

    public static BookingDto CreateMultiDayDtoWithEndpoints(int numberOfDays, DateTime scheduledDateTime)
    {
        var bookingDto = CreateHearingDtoWithEndpoints(JudgePersonalCode, JudgeUsername, false, scheduledDateTime);
        bookingDto.EndDateTime = scheduledDateTime.AddDays(numberOfDays);
        return bookingDto;
    }

    public static BookNewHearingRequestV2 CreateNewRequestDtoWithOnlyAJudge(bool remote = false,
        DateTime? scheduledDateTime = null)
    {
        var date = DateUtil.GetNow(remote);
        var hearingDateTime = scheduledDateTime ?? date.AddMinutes(5);

        var bookingDto = CreateHearingDtoWithOnlyAJudge(scheduledDateTime: hearingDateTime);
        var request = new BookNewHearingRequestV2()
        {
            Cases = new List<CaseRequestV2>()
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
            ServiceId = bookingDto.ServiceId,
            OtherInformation = bookingDto.OtherInformation,
            AudioRecordingRequired = bookingDto.AudioRecording,
            HearingVenueCode = bookingDto.VenueCode,
            JudicialOfficeHolders =
            [
                new()
                {
                    HearingRoleCode = JudiciaryParticipantHearingRoleCode.Judge,
                    DisplayName = bookingDto.Judge.DisplayName,
                    ContactEmail = bookingDto.Judge.Username,
                    PersonalCode = bookingDto.Judge.PersonalCode,
                    ContactTelephone = bookingDto.Judge.Phone
                }
            ],
            BookingSupplier = BookingSupplier.Vodafone,
        };
        return request;
    }
    
    public static BookNewHearingRequestV2 CreateNewRequestDtoJudgeAndEndpointWithLinkedDa(bool remote = false,
        DateTime? scheduledDateTime = null)
    {
        var request = CreateNewRequestDtoWithOnlyAJudge(remote, scheduledDateTime);
        var rep = KnownParticipantsForTesting().First(x => x.Role == GenericTestRole.Representative);
        request.Participants =
        [
            new ParticipantRequestV2
            {
                ContactEmail = rep.ContactEmail,
                DisplayName = rep.DisplayName,
                FirstName = rep.FirstName,
                LastName = rep.LastName,
                ExternalParticipantId = Guid.NewGuid().ToString(),
                HearingRoleCode = HearingRoleCodes.Representative,
                OrganisationName = rep.Organisation,
                Representee = "Auto EP 1"
            }
        ];
        request.Endpoints =
        [
            new EndpointRequestV2
            {
                DisplayName = "Auto EP 1",
                ExternalParticipantId = Guid.NewGuid().ToString(),
                DefenceAdvocateContactEmail = rep.ContactEmail
            }
        ];
        
        return request;
    }
    
    public static class HearingRoleCodes
    {
        public const string Applicant = "APPL";
        public const string Intermediary = "INTE";
        public const string Representative = "RPTT";
        public const string Respondent = "RESP";
        public const string StaffMember = "STAF";
        public const string Interpreter = "INTP";
        public const string WelfareRepresentative = "WERP";
    }
}