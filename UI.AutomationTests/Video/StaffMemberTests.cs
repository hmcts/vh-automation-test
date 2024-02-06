using UI.AutomationTests.TestData;
using UI.AutomationTests.Utilities;

namespace UI.AutomationTests.Video;

public class StaffMemberTests : VideoWebUiTest
{
    [Category("Daily")]
    [Description("Book a hearing. Log into hearing with a staffmember account. Edit their display name. Start and stop the hearing.")]
    [Test]
    public async Task LogIntoHearingWithStaffMember()
    {
        var hearing = await CreateTestHearing();
        TestHearingIds.Add(hearing.Id.ToString());
        var conference = await VideoApiClient.GetConferenceByHearingRefIdAsync(hearing.Id, false);
        TestContext.WriteLine(
            $"Attempting to book a hearing with the case name: {hearing.Cases[0].Name} and case number: {hearing.Cases[0].Number}");

        // log in as staff-member, go to waiting room and start hearing
        var staffMemberVenueList = LoginAsStaffMember(HearingTestData.StaffMemberUsername, EnvConfigSettings.UserPassword);
        var staffMemberHearingList = staffMemberVenueList.SelectHearingsByVenues(hearing.HearingVenueName);
        var statffMemberWaitingRoom = staffMemberHearingList.SelectHearing(conference.Id);
        
        // edit display name
        const string newDisplayName = "Edited Staff Member Name";
        statffMemberWaitingRoom.EditStaffMemberDisplayName(newDisplayName);
        var updatedDisplayName = statffMemberWaitingRoom.GetStaffMemberDisplayNameInWaitingRoom();
        updatedDisplayName.Should().BeEquivalentTo(newDisplayName);

        var smHearingRoomPage = statffMemberWaitingRoom.StartOrResumeHearing();
        statffMemberWaitingRoom.ParticipantExistsInHearingRoom(newDisplayName).Should().BeTrue();
        statffMemberWaitingRoom = smHearingRoomPage.CloseHearing();
        statffMemberWaitingRoom.IsHearingClosed().Should().BeTrue();
        Assert.Pass();
    }

    private async Task<HearingDetailsResponse> CreateTestHearing()
    {
        var hearingScheduledDateAndTime = DateUtil.GetNow(EnvConfigSettings.RunOnSaucelabs).AddMinutes(5);
        var hearingDto = HearingTestData.CreateNewRequestDtoWithOnlyAJudge(scheduledDateTime: hearingScheduledDateAndTime);
        return await BookingsApiClient.BookNewHearingAsync(hearingDto);
    }
}