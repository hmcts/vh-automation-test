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
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, DateTime.Today.AddDays(1).AddHours(10).AddMinutes(00));
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            TestContext.WriteLine(
                $"Attempting to book a hearing with the case name: {hearingDto.CaseName} and case number: {hearingDto.CaseNumber}");

            hearingDto.CaseName += $" Day 1 of {numberOfDays + 1}";
            
            // Change the scheduled datetime
            var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
            var summaryPage = bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);
            hearingDto.ScheduledDateTime = newTime;
            
            // Assign a new Judge 
            var alternativeJudge = new BookingJudgeDto(HearingTestData.AltJudge, "Auto Judge 2", "");
            var assignJudgePage = isV2
                ? summaryPage.ChangeJudgeV2() 
                : summaryPage.ChangeJudgeV1();
            assignJudgePage.EnterJudgeDetails(alternativeJudge, FeatureToggles.UseV2Api());
            hearingDto.Judge = alternativeJudge;
            summaryPage = assignJudgePage.GotToNextPageOnEdit();

            // Add a new participant
            var newParticipant = HearingTestData.CreateNewParticipantDto();
            CreatedUsers.Add(newParticipant.Username);
            var participantsPage = summaryPage.ChangeParticipants(isV2);
            participantsPage.AddNewUserParticipants(new List<BookingParticipantDto>{ newParticipant });
            hearingDto.Participants.Add(newParticipant);
            
            // Update a participant
            var participantToUpdate = hearingDto.Participants.First(p => p.Role == GenericTestRole.Witness);
            var newDisplayName = participantToUpdate.DisplayName + " EDITED";
            participantsPage.UpdateParticipant(participantToUpdate.FullName, " EDITED");
            participantToUpdate.DisplayName = newDisplayName;

            // Remove a participant
            var participantToRemove = hearingDto.Participants.Where(p => p.Role == GenericTestRole.Witness).ToList()[1];
            participantsPage.RemoveParticipant(participantToRemove.FullName);
            hearingDto.Participants.Remove(participantToRemove);
            var videoAccessPointsPage = participantsPage.GoToVideoAccessPointsPage();

            // Add an endpoint
            var newEndpoint = HearingTestData.CreateNewEndpointDto();
            var countOfEndpointsCurrentlyOnHearing = hearingDto.VideoAccessPoints.Count;
            videoAccessPointsPage.AddAnotherVideoAccessPoint(newEndpoint, countOfEndpointsCurrentlyOnHearing);
            hearingDto.VideoAccessPoints.Add(newEndpoint);
            
            // Remove an endpoint
            var sortedEndpoints = hearingDto.VideoAccessPoints.OrderBy(x => x.DisplayName).ToList();
            const int endpointToRemoveIndex = 0;
            var endpointToRemove = sortedEndpoints[endpointToRemoveIndex];
            videoAccessPointsPage.RemoveVideoAccessPoint(endpointToRemoveIndex);
            hearingDto.VideoAccessPoints.Remove(endpointToRemove);
            
            // Change the rep linked to an endpoint
            sortedEndpoints = hearingDto.VideoAccessPoints.OrderBy(x => x.DisplayName).ToList();
            const int endpointIndexToUpdate = 0;
            var endpointToUpdate = sortedEndpoints[endpointIndexToUpdate];
            videoAccessPointsPage.UpdateVideoAccessPoint(endpointIndexToUpdate, "None");
            endpointToUpdate.DefenceAdvocateDisplayName = "";
            
            summaryPage = videoAccessPointsPage.GoToSummaryPage();
            
            // Change the recording setting and other information
            const bool newAudioRecordingSetting = true;
            var newOtherInformation = hearingDto.OtherInformation + " EDITED";
            var otherInformationPage = summaryPage.ChangeOtherInformation();
            otherInformationPage.TurnOnAudioRecording();
            otherInformationPage.EnterOtherInformation(" EDITED");
            hearingDto.AudioRecording = newAudioRecordingSetting;
            hearingDto.OtherInformation = newOtherInformation;
            summaryPage = otherInformationPage.GoToSummaryPage();
            
            summaryPage.ValidateSummaryPage(hearingDto);
            var confirmationPage = summaryPage.ClickBookButton();
            
            confirmationPage.IsBookingSuccessful().Should().BeTrue();
            var bookingDetailPage = confirmationPage.ClickViewBookingLink();
            bookingDetailPage.ValidateDetailsPage(hearingDto);
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
