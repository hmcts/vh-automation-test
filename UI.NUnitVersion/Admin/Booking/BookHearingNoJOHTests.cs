namespace UI.NUnitVersion.Admin.Booking;

public class BookHearingNoJOHTests : AdminWebUiTest
{
    private BookingDto _bookingDto;
    private string _hearingIdString;

    [Category("Daily")]
    [Test]
    public void BookAHearingNoJOH()
    {
        var v2Flag = FeatureToggles.UseV2Api();
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        _bookingDto = HearingTestData.CreateHearingDtoWithEndpoints(
            judgeUsername: FeatureToggles.UseV2Api() ? HearingTestData.EJUD_Judge : HearingTestData.Judge, 
            scheduledDateTime: date);
        _bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var preBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var preBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var preBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var preBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        
        if(v2Flag)
            createHearingPage.EnterHearingDetailsV2(_bookingDto.CaseNumber, _bookingDto.CaseName, _bookingDto.CaseType);
        else
            createHearingPage.EnterHearingDetails(_bookingDto.CaseNumber, _bookingDto.CaseName, _bookingDto.CaseType, _bookingDto.HearingType);
        
        var hearingSchedulePage = createHearingPage.GoToNextPage();
        
        hearingSchedulePage.EnterSingleDayHearingSchedule(
            _bookingDto.ScheduledDateTime, 
            _bookingDto.DurationHour,
            _bookingDto.DurationMinute, 
            _bookingDto.VenueName, 
            _bookingDto.RoomName);
        
        var assignJudgePage = hearingSchedulePage.GoToNextPage();

        if (v2Flag)
        {
            assignJudgePage.ClickContinueWithoutJudiciary();
        }
        else
            assignJudgePage.EnterJudgeDetails(_bookingDto.Judge.Username, _bookingDto.Judge.DisplayName, _bookingDto.Judge.Phone);
            
        var addParticipantPage = assignJudgePage.GoToParticipantsPage(useParty: !v2Flag);
        
        if(v2Flag)
            addParticipantPage.AddExistingParticipantsV2(_bookingDto.Participants);
        else
            addParticipantPage.AddExistingParticipants(_bookingDto.Participants);
        
        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        videoAccessPointsPage.AddVideoAccessPoints(_bookingDto.VideoAccessPoints);
        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);
        
        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryNoJudgePage(_bookingDto);
        
        var confirmationPage = summaryPage.ClickBookButton();
        _hearingIdString = confirmationPage.GetNewHearingId();
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        confirmationPage.ClickViewBookingLink().ValidateDetailsPage(_bookingDto);
        dashboardPage = confirmationPage.GoToDashboardPage();
        
        var postBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var postBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var postBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var postBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();
        
        postBookingUnallocatedHearingsToday.Should().Be(preBookingUnallocatedHearingsToday);
        postBookingUnallocatedHearingsTomorrow.Should().BeGreaterThan(preBookingUnallocatedHearingsTomorrow);
        postBookingUnallocatedHearingsNextSevenDays.Should().BeGreaterThan(preBookingUnallocatedHearingsNextSevenDays);
        postBookingUnallocatedHearingsNextThirtyDays.Should()
            .BeGreaterThan(preBookingUnallocatedHearingsNextThirtyDays);
        
        dashboardPage.SignOut();

        Assert.Pass();
    }
    

    protected override async Task CleanUp()
    {
        if(_hearingIdString != null)
        {
            var hearingId = Guid.Parse(_hearingIdString);
            TestContext.WriteLine($"Removing Hearing {hearingId}");
            await BookingsApiClient.RemoveHearingAsync(hearingId);
            _hearingIdString = null;
        }
    }
}