using UI.AutomationTests.EmailNotifications;
using UI.Common.Utilities;

namespace UI.AutomationTests.Admin.Booking;

public class BookHearingTests : AdminWebUiTest
{
    private BookingDto _bookingDto;

    [Test(Description = "Book a hearing")]
    [Category("admin")]
    [Category("coreAdmin")]
    public async Task BookAHearing()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        _bookingDto = HearingTestData.CreateHearingDtoWithEndpoints(HearingTestData.JudgePersonalCode,
            judgeUsername: HearingTestData.JudgeUsername, scheduledDateTime: date);
        _bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";
        var newUser = HearingTestData.CreateNewParticipantDto();
        CreatedUsers.Add(newUser.Username);
        _bookingDto.NewParticipants.Add(newUser);

        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var preBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var preBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var preBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var preBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();

        var createHearingPage = dashboardPage.GoToBookANewHearing();
        createHearingPage.EnterHearingDetails(_bookingDto);

        var hearingSchedulePage = createHearingPage.GoToNextPage();

        hearingSchedulePage.EnterSingleDayHearingSchedule(_bookingDto);

        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails(_bookingDto.Judge);

        var addParticipantPage = assignJudgePage.GotToNextPage();

        addParticipantPage.AddAllParticipantsFromDto(_bookingDto);

        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        videoAccessPointsPage.AddVideoAccessPoints(_bookingDto.VideoAccessPoints);
        
        var otherInformationPage = FeatureToggle.Instance().SpecialMeasuresEnabled() ?
            videoAccessPointsPage.GoToSpecialMeasuresPage().GoToOtherInformationPage() :
            videoAccessPointsPage.GoToOtherInformationPage();
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);

        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryPage(_bookingDto);

        var confirmationPage = summaryPage.ClickBookButton();
        TestHearingIds.Add(confirmationPage.GetNewHearingId());
        confirmationPage.IsBookingSuccessful().Should().BeTrue();
        
        confirmationPage.ClickViewBookingLink().ValidateDetailsPage(_bookingDto);
        dashboardPage = confirmationPage.GoToDashboardPage();

        var postBookingUnallocatedHearingsToday = dashboardPage.GetNumberOfUnallocatedHearingsToday();
        var postBookingUnallocatedHearingsTomorrow = dashboardPage.GetNumberOfUnallocatedHearingsTomorrow();
        var postBookingUnallocatedHearingsNextSevenDays = dashboardPage.GetNumberOfUnallocatedHearingsNextSevenDays();
        var postBookingUnallocatedHearingsNextThirtyDays = dashboardPage.GetNumberOfUnallocatedHearingsNextThirtyDays();

        postBookingUnallocatedHearingsToday.Should().BeGreaterThanOrEqualTo(preBookingUnallocatedHearingsToday);
        postBookingUnallocatedHearingsTomorrow.Should().BeGreaterThan(preBookingUnallocatedHearingsTomorrow);
        postBookingUnallocatedHearingsNextSevenDays.Should().BeGreaterThan(preBookingUnallocatedHearingsNextSevenDays);
        postBookingUnallocatedHearingsNextThirtyDays.Should()
            .BeGreaterThan(preBookingUnallocatedHearingsNextThirtyDays);

        await ValidateEmailNotifications(newUser, _bookingDto.CaseName, _bookingDto.CaseNumber);
        
        dashboardPage.SignOut();

        Assert.Pass("Hearing booked successfully with existing and a new participant. Unallocated hearings count increased as expected.");
    }

    [TestCase("British Sign Language (BSL)", InterpreterType.Sign)]
    [TestCase("Spanish", InterpreterType.Verbal)]
    [FeatureToggleSetting(FeatureToggle.InterpreterEnhancementsToggleKey, true)]
    [Category("admin")]
    public void BookAHearingWithInterpreterLanguages(string description, InterpreterType type)
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        var interpreterLanguage = new InterpreterLanguageDto(description, type);
        _bookingDto = HearingTestData.CreateHearingDtoWithInterpreterLanguages(HearingTestData.JudgePersonalCode,
            judgeUsername: HearingTestData.JudgeUsername, scheduledDateTime: date, interpreterLanguage);
        _bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";

        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);

        var createHearingPage = dashboardPage.GoToBookANewHearing();
        createHearingPage.EnterHearingDetails(_bookingDto);

        var hearingSchedulePage = createHearingPage.GoToNextPage();
        hearingSchedulePage.EnterSingleDayHearingSchedule(_bookingDto);

        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails(_bookingDto.Judge);

        var addParticipantPage = assignJudgePage.GotToNextPage();
        addParticipantPage.AddAllParticipantsFromDto(_bookingDto);

        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        videoAccessPointsPage.AddVideoAccessPoints(_bookingDto.VideoAccessPoints);
        
        var otherInformationPage = videoAccessPointsPage.GoToSpecialMeasuresPage().GoToOtherInformationPage();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);

        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryPage(_bookingDto);

        var confirmationPage = summaryPage.ClickBookButton();
        TestHearingIds.Add(confirmationPage.GetNewHearingId());
        confirmationPage.IsBookingSuccessful().Should().BeTrue();

        confirmationPage.ClickViewBookingLink().ValidateDetailsPage(_bookingDto);

        Assert.Pass($"Booked a hearing with interpretation for {description}");
    }

    [Test(Description = "Book a hearing with screening")]
    [FeatureToggleSetting(FeatureToggle.SpecialMeasuresKey, true)]
    [Category("admin")]
    public void BookAHearingWithScreening()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        _bookingDto = HearingTestData.CreateHearingDtoWithEndpoints(HearingTestData.JudgePersonalCode,
            judgeUsername: HearingTestData.JudgeUsername, scheduledDateTime: date);
        _bookingDto.CaseNumber = $"Automation Test Hearing - BookAHearing {Guid.NewGuid():N}";
        _bookingDto.Participants[0].Screening = new ScreeningDto
        {
            ProtectedFrom = [new ScreeningEntityDto(_bookingDto.Participants[1].DisplayName)]
        };
        _bookingDto.VideoAccessPoints[0].Screening = new ScreeningDto
        {
            ProtectedFrom = [new ScreeningEntityDto(_bookingDto.VideoAccessPoints[1].DisplayName)] 
        };
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        var createHearingPage = dashboardPage.GoToBookANewHearing();
        createHearingPage.EnterHearingDetails(_bookingDto);

        var hearingSchedulePage = createHearingPage.GoToNextPage();
        hearingSchedulePage.EnterSingleDayHearingSchedule(_bookingDto);

        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails(_bookingDto.Judge);

        var addParticipantPage = assignJudgePage.GotToNextPage();
        addParticipantPage.AddAllParticipantsFromDto(_bookingDto);

        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        videoAccessPointsPage.AddVideoAccessPoints(_bookingDto.VideoAccessPoints);

        var specialMeasuresPage = videoAccessPointsPage.GoToSpecialMeasuresPage();
        specialMeasuresPage.AddScreeningParticipants(_bookingDto.ScreeningParticipants);
        
        var otherInformationPage = specialMeasuresPage.GoToOtherInformationPage();
        if (_bookingDto.AudioRecording)
            otherInformationPage.TurnOnAudioRecording();
        else
            otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(_bookingDto.OtherInformation);
        
        var summaryPage = otherInformationPage.GoToSummaryPage();
        summaryPage.ValidateSummaryPage(_bookingDto);

        var confirmationPage = summaryPage.ClickBookButton();
        TestHearingIds.Add(confirmationPage.GetNewHearingId());
        confirmationPage.IsBookingSuccessful().Should().BeTrue();

        confirmationPage.ClickViewBookingLink().ValidateDetailsPage(_bookingDto);

        Assert.Pass($"Booked a hearing with screening");
    }
    
    private async Task ValidateEmailNotifications(BookingParticipantDto newUser, string caseName, string caseNumber)
    {
        await EmailNotificationService.PullNotificationList();
        //Validate Judge email notification
        await EmailNotificationService.ValidateEmailReceived(_bookingDto.Judge.Username, EmailTemplates.JudgeHearingConfirmation, caseName, caseNumber);
        //Validate New User Participant email notification
        await EmailNotificationService.ValidateEmailReceived(newUser.ContactEmail, EmailTemplates.FirstEmailAllNewUsers, caseName, caseNumber);
        await EmailNotificationService.ValidateEmailReceived(newUser.ContactEmail, EmailTemplates.SecondEmailNewUserConfirmation, caseName, caseNumber);
        //Validate Other Participants email notification
        foreach (var participant in _bookingDto.Participants)
        {
            if (participant.Role == GenericTestRole.Representative)
                await EmailNotificationService.ValidateEmailReceived(participant.ContactEmail, EmailTemplates.ExistingProfessionalConfirmation, caseName, caseNumber);
            else
                await EmailNotificationService.ValidateEmailReceived(participant.ContactEmail, EmailTemplates.ExistingParticipantConfirmation, caseName, caseNumber);
        }
    }
}