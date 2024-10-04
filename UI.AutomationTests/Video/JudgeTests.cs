
using BookingsApi.Contract.V1.Requests.Enums;

namespace UI.AutomationTests.Video
{
    public class JudgeTests : VideoWebUiTest
    {
        
        [Description("Book a hearing. Log into hearing with a judge account. Edit their display name. Start and stop the hearing.")]
        [Test]
        public async Task LogIntoHearingWithJudgeAndEditDisplayName()
        {
            var hearing = await CreateTestHearing();
            TestHearingIds.Add(hearing.Id.ToString());
            var conference = await GetConference(hearing.Id);
            await TestContext.Out.WriteLineAsync(
                $"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");

            // log in as judge, go to waiting room and start hearing
            var judgeUsername = hearing.JudiciaryParticipants
                .Find(x => x.HearingRoleCode == JudiciaryParticipantHearingRoleCode.Judge).Email;
            var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
            var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        
            // edit display name
            const string newDisplayName = "Edited Judge Name Automated Test";
            judgeWaitingRoomPage.EditJudgeDisplayName(newDisplayName);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            judgeWaitingRoomPage.ParticipantExistsInWaitingRoom(newDisplayName).Should().BeTrue();

            var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
            judgeWaitingRoomPage.ParticipantExistsInHearingRoom(newDisplayName).Should().BeTrue();
            judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
            judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();
            Assert.Pass();
        }

        [Test]
        public async Task PauseHearingDuringCountdown()
        {
            var hearing = await CreateTestHearing();
            TestHearingIds.Add(hearing.Id.ToString());
            var conference = await GetConference(hearing.Id);
            await TestContext.Out.WriteLineAsync(
                $"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");

            // log in as judge, go to waiting room and start hearing
            var judgeUsername = hearing.JudiciaryParticipants
                .Find(x => x.HearingRoleCode == JudiciaryParticipantHearingRoleCode.Judge).Email;
            var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
            var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);

            var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
            
            Thread.Sleep(TimeSpan.FromSeconds(10)); // Allow time for the countdown to start
            judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
            judgeWaitingRoomPage.IsHearingPaused().Should().BeTrue();
            Assert.Pass();
        }

        private async Task<HearingDetailsResponseV2> CreateTestHearing()
        {
            var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
            var hearingDto = HearingTestData.CreateNewRequestDtoWithOnlyAJudge(scheduledDateTime: hearingScheduledDateAndTime);
            return await BookingsApiClient.BookNewHearingWithCodeAsync(hearingDto);
        }
    }
}
