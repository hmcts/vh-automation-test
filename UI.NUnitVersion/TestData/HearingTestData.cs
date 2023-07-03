using UI.NUnitVersion.Utilities;
using UI.PageModels.Dtos;

namespace UI.NUnitVersion.TestData;

public static class HearingTestData
{
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
            CaseName = $"BookAHearing Automation Test {date:M-d-yy-H-mm-ss}",
            CaseNumber = "Automation Test Hearing",
            CaseType = "Civil",
            HearingType = "Enforcement Hearing",
            ScheduledDateTime = hearingDateTime,
            DurationHour = 1,
            DurationMinute = 30,
            VenueName = "Birmingham Civil and Family Justice Centre",
            RoomName = "Room 1",
            Judge = new BookingJudgeDto
            {
                Username = "auto_aw.judge_02@hearings.reform.hmcts.net",
                DisplayName = "Auto Judge",
                Phone = ""
            },
            Participants = new List<BookingExistingParticipantDto>
            {
                BookingExistingParticipantDto.Individual(GenericTestParty.Claimant, GenericTestRole.LitigantInPerson,
                    "auto_vw.individual_60@hmcts.net", "auto_vw.individual_60@hearings.reform.hmcts.net", "Auto 1"),
                BookingExistingParticipantDto.Representative(GenericTestParty.Claimant, GenericTestRole.Representative,
                    "auto_vw.representative_139@hmcts.net", "auto_vw.representative_139@hearings.reform.hmcts.net",
                    "Auto 2", "Auto 1"),
                BookingExistingParticipantDto.Individual(GenericTestParty.Defendant, GenericTestRole.LitigantInPerson,
                    "auto_vw.individual_137@hmcts.net", "auto_vw.individual_137@hearings.reform.hmcts.net", "Auto 3"),
                BookingExistingParticipantDto.Representative(GenericTestParty.Defendant, GenericTestRole.Representative,
                    "auto_vw.representative_157@hmcts.net", "auto_vw.representative_157@hearings.reform.hmcts.net",
                    "Auto 4",
                    "Auto 3")
            }
        };
        return bookingDto;
    }
}