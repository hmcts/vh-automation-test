using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Admin.Booking
{
    public class EditMultiDayHearingTests : AdminWebUiTest
    {
        private string _groupId;
        
        [Category("Daily")]
        [Test]
        public void EditSingleDayOfMultiDayHearing()
        {
            const int numberOfDays = 3;
            var hearingDto = HearingTestData.CreateMultiDayDto(numberOfDays, DateTime.Today.AddDays(1).AddHours(9).AddMinutes(00));
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            TestContext.WriteLine(
                $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");

            // Change the scheduled datetime
            var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
            var summaryPage = bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);
            
            // Assign a new Judge 
            var alternativeJudge = new BookingJudgeDto(HearingTestData.AltJudge, "Auto Judge 2", "");
            var assignJudgePage = FeatureToggles.UseV2Api() 
                ? summaryPage.ChangeJudgeV2() 
                : summaryPage.ChangeJudgeV1();
            assignJudgePage.EnterJudgeDetails(alternativeJudge, FeatureToggles.UseV2Api());
            summaryPage = assignJudgePage.GotToNextPageOnEdit();

            // Change the recording setting and other information
            const bool newAudioRecordingSetting = true;
            var newOtherInformation = hearingDto.OtherInformation + " EDITED";
            var otherInformationPage = summaryPage.ChangeOtherInformation();
            otherInformationPage.TurnOnAudioRecording();
            otherInformationPage.EnterOtherInformation(" EDITED");
            otherInformationPage.GoToSummaryPage();
            
            var confirmationPage = summaryPage.ClickBookButton();
            
            confirmationPage.IsBookingSuccessful().Should().BeTrue();
            var bookingDetailPage = confirmationPage.ClickViewBookingLink();
            hearingDto.ScheduledDateTime = newTime;
            hearingDto.Judge = alternativeJudge;
            hearingDto.AudioRecording = newAudioRecordingSetting;
            hearingDto.OtherInformation = newOtherInformation;
            bookingDetailPage.ValidateDetailsPage(hearingDto);
            Assert.Pass();
        }
        
        [TearDown]
        public async Task CleanUpMultiDayHearings()
        {
            if (string.IsNullOrEmpty(_groupId)) return;
            var hearings = await BookingsApiClient.GetHearingsByGroupIdAsync(Guid.Parse(_groupId));
            foreach (var hearingId in hearings.Select(h => h.Id))
                try
                {
                    await BookingsApiClient.RemoveHearingAsync(hearingId);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to remove hearing {hearingId} with exception {e.Message}");
                }
        }

        private BookingDetailsPage BookMultiDayHearingAndGoToDetailsPage(BookingDto bookingDto)
        {
            var driver = VhDriver.GetDriver();
            driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
            var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
            var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
            
            var createHearingPage = dashboardPage.GoToBookANewHearing();
            var summaryPage = createHearingPage.BookAHearingJourney(bookingDto, FeatureToggles.UseV2Api(), isMultiDay: true);
            var confirmationPage = summaryPage.ClickBookButton();
            _groupId = confirmationPage.GetNewHearingId();
            var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
            return bookingDetailsPage;
        }
    }
}
