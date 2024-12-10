using UI.PageModels.Pages.PexipInfinityWeb;

namespace UI.AutomationTests.Video;

public class JoinAsJvsEndpointTest : VideoWebUiTest
{
    [Test]
    [Category("video")]
    [Category("coreVideo")]
    public async Task ConnectToPexipAsJvsEndpoint()
    {
        // Arrange
        var hearing = await CreateTestHearing();
        TestHearingIds.Add(hearing.Id.ToString());
        
        await TestContext.Out.WriteLineAsync(
            $"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");
        // wait until conference is created
        var conference = await GetConference(hearing.Id);
        
        // log in as JVS endpoint
        var endpoint = conference.Endpoints[0];
        var pexipNodeAddress = EnvConfigSettings.PexipNodeAddress;
        
        var displayName = "VH Test";
        var pexipWebAppUrl =
            PexipWebAppUrlBuilder.BuildPexipWebAppUrl(pexipNodeAddress, endpoint.SipAddress, endpoint.Pin, displayName);
        var jvsWebPage = LoginAsJvsEndpoint(pexipWebAppUrl, endpoint.DisplayName);
        jvsWebPage.ClickJoinMeetingButton();

        // log in as judge and start the hearing
        var judgeUsername = hearing.JudicialOfficeHolders[0].Email;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;
        
        judgeWaitingRoomPage.WaitForParticipantToBeConnected(endpoint.DisplayName);
        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        judgeHearingRoomPage.WaitForCountdownToComplete();
        judgeHearingRoomPage.IsParticipantInHearing(endpoint.DisplayName).Should().BeTrue();

        judgeHearingRoomPage.DismissParticipant(endpoint.DisplayName, endpoint.Id.ToString());
        judgeHearingRoomPage.IsParticipantInHearing(endpoint.DisplayName).Should().BeFalse();
        
        judgeHearingRoomPage.AdmitParticipant(endpoint.DisplayName, endpoint.Id.ToString());
        judgeHearingRoomPage.IsParticipantInHearing(endpoint.DisplayName).Should().BeTrue();
        
        judgeWaitingRoomPage = judgeHearingRoomPage.CloseHearing();
        judgeWaitingRoomPage.IsHearingClosed().Should().BeTrue();
        
        judgeWaitingRoomPage.GetConsultationCloseTime().Should()
            .MatchRegex(@"The consultation room will close at \d{2}:\d{2}");
        
        Assert.Pass("Logged in as JVS endpoint and connected to the hearing");
    }
    
    private async Task<HearingDetailsResponseV2> CreateTestHearing()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSauceLabs || EnvConfigSettings.RunHeadlessBrowser).AddMinutes(5);
        var hearingDto = HearingTestData.CreateNewRequestDtoJudgeAndEndpoint(scheduledDateTime: hearingScheduledDateAndTime);
        return await BookingsApiClient.BookNewHearingWithCodeAsync(hearingDto);
    }
}