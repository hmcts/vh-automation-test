using UI.Common.Utilities;
using UI.PageModels.Pages.Admin.Booking;

namespace UI.AutomationTests.Admin.Booking
{
    public class EditMultiDayHearingTests : MultiDayHearingTest
    {
        
        [Test]
        [Category("admin")]
        public void EditSingleDayOfMultiDayHearing()
        {
            const int numberOfDays = 3;
            var scheduledDateTime = GetFirstDayOfNextWeek(DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs)).Date
                .AddHours(10).AddMinutes(0);
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, scheduledDateTime);
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            UpdateCaseName(hearingDto, numberOfDays);
            
            // Change the scheduled datetime
            var newTime = hearingDto.ScheduledDateTime.AddMinutes(30);
            var summaryPage = bookingDetailsPage.UpdateSchedule(newTime, hearingDto.DurationHour, hearingDto.DurationMinute);
            hearingDto.ScheduledDateTime = newTime;
            
            // Change everything else
            summaryPage = EditMultiDayHearing(hearingDto, summaryPage);
            
            summaryPage.ValidateSummaryPage(hearingDto);
            var confirmationPage = summaryPage.ClickBookButton();
            
            confirmationPage.IsBookingSuccessful().Should().BeTrue();
            var bookingDetailPage = confirmationPage.ClickViewBookingLink();
            bookingDetailPage.ValidateDetailsPage(hearingDto);
            Assert.Pass();
        }

        
        [Test]
        [Category("admin")]
        [FeatureToggleSetting(FeatureToggle.MultiDayBookingEnhancementsToggleKey, true)]
        public async Task EditThisAndUpcomingDaysOfMultiDayHearing()
        {
            const int numberOfDays = 3;
            var scheduledDateTime = GetFirstDayOfNextWeek(DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs)).Date
                .AddHours(10).AddMinutes(0);
            var hearingDto = HearingTestData.CreateMultiDayDtoWithEndpoints(numberOfDays, scheduledDateTime);
            var bookingDetailsPage = BookMultiDayHearingAndGoToDetailsPage(hearingDto);
            UpdateCaseName(hearingDto, numberOfDays);
            
            // Change the scheduled datetimes
            var newDates = new List<DateTime>();
            for (var i = 1; i <= numberOfDays + 1; i++)
            {
                // Move to the following week
                newDates.Add(hearingDto.ScheduledDateTime.AddDays(i + 6));
            }
            var newStartTime = hearingDto.ScheduledDateTime.TimeOfDay.Add(TimeSpan.FromHours(1));
            var summaryPage = bookingDetailsPage.UpdateScheduleForMultipleHearings(newDates, newStartTime.Hours, newStartTime.Minutes, hearingDto.DurationHour, hearingDto.DurationMinute);
            hearingDto.ScheduledDateTime = newDates[0].Date.Add(newStartTime);
            hearingDto.EndDateTime = newDates[^1].Date.Add(newStartTime);

            // Change everything else
            summaryPage = EditMultiDayHearing(hearingDto, summaryPage);

            summaryPage.ValidateSummaryPage(hearingDto, isMultiDay: true);
            var confirmationPage = summaryPage.ClickBookButton();
            
            confirmationPage.IsBookingSuccessful().Should().BeTrue();
            await Task.Delay(10000); // Allow time for new users to be created
            var bookingDetailPage = confirmationPage.ClickViewBookingLink();
            bookingDetailPage.ValidateDetailsPage(hearingDto);
            
            // Return to the booking list and validate the details page for each of the subsequent days in the multi-day hearing
            foreach (var newDate in newDates)
            {
                hearingDto.ScheduledDateTime = newDate;
                bookingDetailsPage = SearchAndViewHearing(bookingDetailPage, hearingDto);
                bookingDetailsPage.ValidateDetailsPage(hearingDto);
            }
            
            Assert.Pass();
        }

        private SummaryPage EditMultiDayHearing(BookingDto hearingDto, SummaryPage summaryPage)
        {
            // Assign a new Judge 
            var alternativeJudge = new BookingJudgeDto(HearingTestData.JudgePersonalCode,
                HearingTestData.AltJudgeUsername, "Auto Judge 2", "");
            var assignJudgePage = summaryPage.ChangeJudgeV2();
            assignJudgePage.EnterJudgeDetails(alternativeJudge);
            hearingDto.Judge = alternativeJudge;
            summaryPage = assignJudgePage.GotToNextPageOnEdit();

            // Add a new participant
            var newParticipant = HearingTestData.CreateNewParticipantDto();
            CreatedUsers.Add(newParticipant.Username);
            var participantsPage = summaryPage.ChangeParticipants();
            participantsPage.AddNewUserParticipants([newParticipant]);
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
            videoAccessPointsPage.AddVideoEndpoint(newEndpoint.DisplayName, newEndpoint.DefenceAdvocateDisplayName);
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
            
            // Go to the summary page
            // The journey is slightly different here when special measures is enabled
            var specialMeasuresEnabled = FeatureToggle.Instance().SpecialMeasuresEnabled();
            if (specialMeasuresEnabled)
                summaryPage = videoAccessPointsPage.GoToSummaryPage();
            else
                summaryPage = videoAccessPointsPage.GoToOtherInformationPage().GoToSummaryPage();
            
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
    }
}