using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Admin.Booking
{
    public class EditMultiDayHearingTests : AdminWebUiTest
    {
        [Category("Daily")]
        [Test]
        public void EditSingleDayOfMultiDayHearing()
        {
            var isV2 = FeatureToggles.UseV2Api();
            const int numberOfDays = 3;
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, DateTime.Today.AddDays(1).AddHours(10).AddMinutes(00));
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            UpdateCaseName(hearingDto, numberOfDays);
            
            // Change the scheduled datetime
            var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
            var summaryPage = bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);
            hearingDto.ScheduledDateTime = newTime;
            
            // Change everything else
            summaryPage = EditMultiDayHearing(isV2, hearingDto, summaryPage);
            
            summaryPage.ValidateSummaryPage(hearingDto);
            var confirmationPage = summaryPage.ClickBookButton();
            
            confirmationPage.IsBookingSuccessful().Should().BeTrue();
            var bookingDetailPage = confirmationPage.ClickViewBookingLink();
            bookingDetailPage.ValidateDetailsPage(hearingDto);
            Assert.Pass();
        }

        [Category("Daily")]
        [Test]
        public async Task EditThisAndUpcomingDaysOfMultiDayHearing()
        {
            var isV2 = FeatureToggles.UseV2Api();
            var multiDayBookingEnhancementsEnabled = FeatureToggles.MultiDayBookingEnhancementsEnabled();
            if (!isV2 || !multiDayBookingEnhancementsEnabled)
                Assert.Pass("V2 and multi day booking enhancements are not both enabled, cannot edit this and upcoming hearings. Skipping Test");
            
            const int numberOfDays = 3;
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, DateTime.Today.AddDays(1).AddHours(10).AddMinutes(00));
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            UpdateCaseName(hearingDto, numberOfDays);
            
            // Change the scheduled datetimes
            var newDates = new List<DateTime>();
            for (var i = 1; i <= numberOfDays + 1; i++)
            {
                newDates.Add(hearingDto.ScheduledDateTime.AddDays(i));
            }
            var newStartTime = hearingDto.ScheduledDateTime.TimeOfDay.Add(TimeSpan.FromHours(1));
            var summaryPage = bookingDetailsPage.UpdateScheduleForMultipleHearings(newDates, newStartTime.Hours, newStartTime.Minutes, hearingDto.DurationHour, hearingDto.DurationMinute);
            hearingDto.ScheduledDateTime = newDates[0].Date.Add(newStartTime);
            hearingDto.EndDateTime = newDates[^1].Date.Add(newStartTime);

            // Change everything else
            summaryPage = EditMultiDayHearing(isV2: true, hearingDto, summaryPage);

            summaryPage.ValidateSummaryPage(hearingDto, isMultiDay: true);
            var confirmationPage = summaryPage.ClickBookButton();
            
            confirmationPage.IsBookingSuccessful().Should().BeTrue();
            await Task.Delay(10000); // Allow time for new users to be created
            var bookingDetailPage = confirmationPage.ClickViewBookingLink();
            bookingDetailPage.ValidateDetailsPage(hearingDto);
            
            // Return to the booking list and validate the details page for each of the subsequent days in the multi-day hearing
            var driver = VhDriver.GetDriver();
            for (var i = 2; i <= numberOfDays + 1; i++)
            {
                hearingDto.ScheduledDateTime = hearingDto.ScheduledDateTime.AddDays(1);
                SearchAndValidateHearing(driver, hearingDto);
            }
            
            Assert.Pass();
        }

        private SummaryPage EditMultiDayHearing(bool isV2, BookingDto hearingDto, SummaryPage summaryPage)
        {
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

            return summaryPage;
        }

        private BookingDetailsPage BookMultiDayHearingAndGoToDetailsPage(BookingDto bookingDto)
        {
            TestContext.WriteLine(
                $"Attempting to book a hearing with the case name: {bookingDto.CaseName} and case number: {bookingDto.CaseNumber}");
            
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

        private void SearchAndValidateHearing(IWebDriver driver, BookingDto hearingDto)
        {
            // Search for the hearing on the booking list page
            driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
            var dashboardPage = new DashboardPage(driver, EnvConfigSettings.DefaultElementWait);
            var bookingListPage = dashboardPage.GoToBookingList();
            var queryDto = new BookingListQueryDto
            {
                CaseNumber = hearingDto.CaseNumber,
                StartDate = hearingDto.ScheduledDateTime
            };
            bookingListPage.SearchForBooking(queryDto);
            
            // Validate the details
            var bookingDetailPage = bookingListPage.ViewBookingDetails(queryDto.CaseNumber);
            bookingDetailPage.ValidateDetailsPage(hearingDto);
        }

        /// <summary>
        /// Multi day hearings include the day number in their case name
        /// </summary>
        /// <param name="bookingDto"></param>
        /// <param name="numberOfDays"></param>
        private static void UpdateCaseName(BookingDto bookingDto, int numberOfDays)
        {
            bookingDto.CaseName += $" Day 1 of {numberOfDays + 1}";
        }
    }
}
