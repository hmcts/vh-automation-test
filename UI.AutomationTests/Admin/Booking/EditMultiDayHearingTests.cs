using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Admin.Booking
{
    public class EditMultiDayHearingTests : AdminWebUiTest
    {
        [Category("Daily")]
        [Test]
        public void EditSingleDayOfMultiDayHearing()
        {
            const int numberOfDays = 3;
            var isV2 = FeatureToggles.UseV2Api();
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, DateTime.Today.AddDays(1).AddHours(9).AddMinutes(00));
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            TestContext.WriteLine(
                $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");

            // Change the scheduled datetime
            var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
            var summaryPage = bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);
            
            // Assign a new Judge 
            var alternativeJudge = new BookingJudgeDto(HearingTestData.AltJudge, "Auto Judge 2", "");
            var assignJudgePage = isV2
                ? summaryPage.ChangeJudgeV2() 
                : summaryPage.ChangeJudgeV1();
            assignJudgePage.EnterJudgeDetails(alternativeJudge, FeatureToggles.UseV2Api());
            summaryPage = assignJudgePage.GotToNextPageOnEdit();

            // Add a new participant
            var newParticipant = HearingTestData.CreateNewParticipantDto();
            CreatedUsers.Add(newParticipant.Username);
            var participantsPage = summaryPage.ChangeParticipants(isV2);
            participantsPage.AddNewUserParticipants(new List<BookingParticipantDto>{ newParticipant });
            // TODO update the hearing dto so that we can validate later
            
            // Update a participant
            const int participantToUpdateIndex = 1;
            const string displayNameSuffix = " EDITED";
            participantsPage.UpdateParticipant(participantToUpdateIndex, displayNameSuffix);
            // TODO update the hearing dto so that we can validate later

            // Remove a participant
            const int participantToRemoveIndex = 2;
            participantsPage.RemoveParticipant(participantToRemoveIndex);
            var videoAccessPointsPage = participantsPage.GoToVideoAccessPointsPage();
            // TODO update the hearing dto so that we can validate later

            // Add an endpoint
            var newEndpoint = HearingTestData.CreateNewEndpointDto();
            var countOfEndpointsCurrentlyOnHearing = hearingDto.VideoAccessPoints.Count;
            videoAccessPointsPage.AddAnotherVideoAccessPoint(newEndpoint, countOfEndpointsCurrentlyOnHearing);
            
            // Remove an endpoint
            const int endpointToRemoveIndex = 0;
            videoAccessPointsPage.RemoveVideoAccessPoint(endpointToRemoveIndex);
            
            // Change the rep linked to an endpoint
            const int endpointIndexToUpdate = 0;
            videoAccessPointsPage.UpdateVideoAccessPoint(endpointIndexToUpdate, "None");
            
            summaryPage = videoAccessPointsPage.GoToSummaryPage();
            
            // Change the recording setting and other information
            const bool newAudioRecordingSetting = true;
            var newOtherInformation = hearingDto.OtherInformation + " EDITED";
            var otherInformationPage = summaryPage.ChangeOtherInformation();
            otherInformationPage.TurnOnAudioRecording();
            otherInformationPage.EnterOtherInformation(" EDITED");
            summaryPage = otherInformationPage.GoToSummaryPage();
            
            //summaryPage.ValidateSummaryPage(hearingDto, isMultiDay: true);
            var confirmationPage = summaryPage.ClickBookButton();
            
            confirmationPage.IsBookingSuccessful().Should().BeTrue();
            var bookingDetailPage = confirmationPage.ClickViewBookingLink();
            hearingDto.ScheduledDateTime = newTime;
            hearingDto.Judge = alternativeJudge;
            hearingDto.AudioRecording = newAudioRecordingSetting;
            hearingDto.OtherInformation = newOtherInformation;
            //bookingDetailPage.ValidateDetailsPage(hearingDto);
            Assert.Pass();
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
            TestHearingIds.Add(confirmationPage.GetNewHearingId());
            var bookingDetailsPage = confirmationPage.ClickViewBookingLink();
            return bookingDetailsPage;
        }
    }
}
