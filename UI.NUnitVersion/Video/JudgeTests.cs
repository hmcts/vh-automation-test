namespace UI.NUnitVersion.Video
{
    public class JudgeTests : VideoWebUiTest
    {
        [Category("Daily")]
        [Description("Book a hearing. Log into hearing with a judge account. Edit their display name. Start and stop the hearing.")]
        [Test]
        public async Task LogIntoHearingWithJudge()
        {
            var hearing = await CreateTestHearing();
            var conference = await VideoApiClient.GetConferenceByHearingRefIdAsync(hearing.Id, false);
            TestContext.WriteLine(
                $"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");

            // log in as judge, go to waiting room and start hearing
            var judgeUsername = hearing.Participants.Find(x => x.HearingRoleName == "Judge")?.Username;
            var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
            var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        
            // edit display name
            judgeWaitingRoomPage.EditJudgeDisplayName();

            var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
            judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
            judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();
            Assert.Pass();
        }

        private async Task<HearingDetailsResponse> CreateTestHearing()
        {
            var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
            var hearingDto = HearingTestData.CreateNewRequestDtoWithOnlyAJudge(scheduledDateTime: hearingScheduledDateAndTime);
            return await BookingsApiClient.BookNewHearingAsync(hearingDto);
        }
    }
}