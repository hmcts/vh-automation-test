using BookingsApi.Contract.V2.Enums;
using UI.PageModels.Pages.Video.Participant;
using VideoApi.Contract.Responses;

namespace UI.AutomationTests.Video
{
    public class RepTests : VideoWebUiTest
    {
        private ConferenceDetailsResponse _conference;

        [Description("Book a hearing. Log into hearing with a Rep. Use Rep Skip self test journey")]
        [Test]
        [Category("video")]
        public async Task RepSkipSelfTestJourney()
        {
            var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
            var request = HearingTestData.CreateNewRequestDtoJudgeAndEndpointWithLinkedParticipants(scheduledDateTime: hearingScheduledDateAndTime);
            var hearing = await BookingsApiClient.BookNewHearingWithCodeAsync(request);
            TestHearingIds.Add(hearing.Id.ToString());
            var conference = await GetConference(hearing.Id);
            _conference = conference;
            await TestContext.Out.WriteLineAsync(
                $"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");
        
            //log in with Rep and skip self test journey
            var rep = hearing.Participants.Find(x => x.HearingRoleCode == HearingTestData.HearingRoleCodes.Representative);
            ValidateRepSelfTestSkipJourney(rep.Username); 
            
            Assert.Pass("Logged in  with REP, skipped self test.");
        }
        
        private void ValidateRepSelfTestSkipJourney(string repUsername, bool failTest = false)
        {
            var participantPassword = EnvConfigSettings.UserPassword;
            var introductionPage = LoginAsParticipant(repUsername, participantPassword, true, null)
                .SelectHearing(_conference.Id);
            if (introductionPage.IsSkipButtonVisible())
            {
                SkipSelfTestJourneyToWaitingRoom(introductionPage, repUsername);
            }
            else if(failTest)
            {
                Assert.Fail("Skip button not visible for Rep, that has already logged in via self test journey.");
            }
            else
            {
                StandardLoginJourneyToWaitingRoom(introductionPage, repUsername);
                ParticipantDrivers[repUsername].Driver.Terminate();
                ParticipantDrivers.Remove(repUsername);
                ValidateRepSelfTestSkipJourney(repUsername, true);
            }
        }
        private void StandardLoginJourneyToWaitingRoom(GetReadyForTheHearingIntroductionPage introductionPage, string participantUsername)
        {
            var participantWaitingRoom = introductionPage
                .GoToEquipmentCheck()
                .GoToSwitchOnCameraMicrophonePage()
                .SwitchOnCameraMicrophone().GoToCameraWorkingPage(true).SelectCameraYes().SelectMicrophoneYes()
                .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration();
            ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
        }
        private void SkipSelfTestJourneyToWaitingRoom(GetReadyForTheHearingIntroductionPage introductionPage, string participantUsername)
        {
            var participantWaitingRoom = introductionPage
                .SkipToCourtRules()
                .AcceptCourtRules()
                .AcceptDeclaration();
            ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
        }
    }
}
