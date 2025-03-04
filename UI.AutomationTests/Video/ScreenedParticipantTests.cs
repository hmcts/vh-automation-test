using UI.Common.Utilities;
using UI.PageModels.Pages.Video.Participant;
using VideoApi.Contract.Responses;

namespace UI.AutomationTests.Video;

public class ScreenedParticipantTests : VideoWebUiTest
{
    private ConferenceDetailsResponse _conference;
    
    [Test]
    [Category("video")]
    [FeatureToggleSetting(FeatureToggle.SpecialMeasuresKey, true)]
    public async Task JoinJohConsultationWithScreenedParticipants()
    {
        // Book a hearing with 2 participants, screened from each other
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDto(HearingTestData.JudgePersonalCode,
            judgeUsername: HearingTestData.JudgeUsername, scheduledDateTime: hearingScheduledDateAndTime);
        var participant1 = hearingDto.Participants[0];
        var participant2 = hearingDto.Participants[1];
        participant1.Screening = new ScreeningDto
        {
            ProtectedFrom = [new ScreeningEntityDto(participant2.DisplayName)] 
        };
        await BookHearing(hearingDto);
        
        // Log in as judge, go to waiting room
        var judgeWaitingRoomPage = JudgeLoginToWaitingRoomJourney(hearingDto, _conference.Id);
        
        // Log in as each participant involved in screening, go to the waiting room
        var participantWaitingRoomPages = new Dictionary<BookingParticipantDto, ParticipantWaitingRoomPage>();
        var screeningParticipants = new List<BookingParticipantDto>{ participant1, participant2 };
        Parallel.ForEach(screeningParticipants, participant =>
        {
            var waitingRoomPage = ParticipantLoginToWaitingRoomJourney(participant, _conference.Id);
            participantWaitingRoomPages.Add(participant, waitingRoomPage);
        });

        // Enter the consultation room as the judge
        var judgeConsultationRoomPage = judgeWaitingRoomPage.JoinJudicialConsultationRoom();
        
        foreach (var participant in screeningParticipants)
        {
            // Admit the participant into the consultation room
            judgeConsultationRoomPage.InviteParticipant(participant.DisplayName);
            var participantConsultationRoomPage = participantWaitingRoomPages[participant].AcceptPrivateConsultation();

            // Verify that their "screening partner" cannot be invited into the room
            var screeningPartner = participant.DisplayName == participant1.DisplayName ? participant2 : participant1;
            var canInvite = judgeConsultationRoomPage.CanInviteParticipant(screeningPartner.DisplayName);
            canInvite.Should().BeFalse();

            // Have the participant leave the consultation room
            participantConsultationRoomPage.LeaveConsultationRoom();
        }

        // Close the hearing and sign out
        judgeWaitingRoomPage = judgeConsultationRoomPage.LeaveJudicialConsultationRoom();
        judgeWaitingRoomPage.StartOrResumeHearing().CloseHearing();
        SignOutAllUsers();
    }
    
    private async Task BookHearing(BookingDto bookingDto)
    {
        var request = HearingTestData.CreateRequest(bookingDto);
        var hearing = await BookingsApiClient.BookNewHearingWithCodeAsync(request);
        TestHearingIds.Add(hearing.Id.ToString());
        _conference = await GetConference(hearing.Id);
    }
}