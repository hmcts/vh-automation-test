using UI.AutomationTests.TestData;
using UI.AutomationTests.Utilities;
using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Video;

public class AddParticipantsPostBookingTests : VideoWebUiTest
{
    private string _hearingIdString;
    
    [Category("Daily")]
    [Description("Book a hearing with a judge and add a participant after booking. Check if the notification appears and if the user joins the judge in the hearing when a hearing is started.")]
    [Test]
    public async Task should_add_new_participant_after_booking()
    {
        var isV2 = FeatureToggles.UseV2Api();
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
        var hearingDto = HearingTestData.CreateHearingDtoWithOnlyAJudge(scheduledDateTime:hearingScheduledDateAndTime);
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");
        
        var bookingDetailsPage = BookHearingAndGoToDetailsPage(hearingDto);
        var conference = await VideoApiClient.GetConferenceByHearingRefIdAsync(new Guid(_hearingIdString) , false);

        // log in as judge, go to waiting room and wait for alerts
        var judgeUsername = hearingDto.Judge.Username;
        var judgeHearingListPage = LoginAsJudge(judgeUsername, EnvConfigSettings.UserPassword);
        var judgeWaitingRoomPage = judgeHearingListPage.SelectHearing(conference.Id);
        ParticipantDrivers[judgeUsername].VhVideoWebPage = judgeWaitingRoomPage;
        
        var participantsToAdd = new List<BookingExistingParticipantDto>(){HearingTestData.KnownParticipantsForTesting()[0]};
        var confirmationPage = bookingDetailsPage.AddParticipantsToBooking(participantsToAdd, !isV2);
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        hearingDto.Participants.AddRange(participantsToAdd);
        
        var participantsInConference = await VideoApiClient.GetParticipantsByConferenceIdAsync(conference.Id);
        // loop through all participants in hearing and login as each one
        foreach (var participant in hearingDto.Participants)
        {
            var participantUsername = participant.Username;
            var participantPassword = EnvConfigSettings.UserPassword;
            var participantHearingList = LoginAsParticipant(participantUsername, participantPassword, participant.Role == GenericTestRole.Representative);
            var participantWaitingRoom = participantHearingList
                .SelectHearing(conference.Id)
                .GoToEquipmentCheck()
                .GoToSwitchOnCameraMicrophonePage()
                .SwitchOnCameraMicrophone().GoToCameraWorkingPage().SelectCameraYes().SelectMicrophoneYes()
                .SelectYesToVisualAndAudioClarity().AcceptCourtRules().AcceptDeclaration(participant.Role == GenericTestRole.Witness);
            // store the participant driver in a dictionary so we can access it later to sign out
            ParticipantDrivers[participantUsername].VhVideoWebPage = participantWaitingRoom;
            
            judgeWaitingRoomPage.WaitForParticipantToBeConnected(participant.FullName);
            
            judgeWaitingRoomPage.ClearParticipantAddedNotification(participant.FullName);
        }

        var judgeHearingRoomPage = judgeWaitingRoomPage.StartOrResumeHearing();
        foreach (var participant in participantsToAdd)
        {
            var p = participantsInConference.First(x => x.Username == participant.Username);
            judgeHearingRoomPage.AdmitParticipant(p.DisplayName, p.Id.ToString());
        }
        
        judgeWaitingRoomPage = judgeHearingRoomPage.PauseHearing();
        judgeWaitingRoomPage.IsHearingPaused().Should().BeTrue();

        Assert.Pass();
    }
    
    private BookingDetailsPage BookHearingAndGoToDetailsPage(BookingDto bookingDto)
    {
        var driver = AdminWebDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();
        var summaryPage = createHearingPage.BookAHearingJourney(bookingDto, FeatureToggles.UseV2Api());
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        TestHearingIds.Add(_hearingIdString);
        TestContext.WriteLine($"Hearing  ID: {_hearingIdString}");
        return confirmationPage.ClickViewBookingLink();
    }
}