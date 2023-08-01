namespace UI.NUnitVersion.Admin.BookingList;

public class SearchForHearingTests : AdminWebUiTest
{
    private HearingDetailsResponse _hearing;

    [Category("Daily")]
    [Test]
    public async Task SearchForHearingViaBookingList()
    {
        await BookBasicHearing();
        
        var driver = VhDriver.GetDriver();
        driver.Navigate().GoToUrl(EnvConfigSettings.AdminUrl);
        var loginPage = new AdminWebLoginPage(driver, EnvConfigSettings.DefaultElementWait);
        var dashboardPage = loginPage.Login(AdminLoginUsername, EnvConfigSettings.UserPassword);
        
        // ideally need to make this test run independently of other tests
        var bookingListPage = dashboardPage.GoToBookingList();
        var queryDto = new BookingListQueryDto()
        {
            CaseNumber = _hearing.Cases[0].Number,
            StartDate = _hearing.ScheduledDateTime.Date,
            EndDate = _hearing.ScheduledDateTime.Date,
            UnallocatedOnly = true
        };
        bookingListPage.SearchForBooking(queryDto);
        var bookingDetailsPage = bookingListPage.ViewBookingDetails(queryDto.CaseNumber);
        bookingDetailsPage.GetAllocatedTo().Should().Be("Not Allocated");
        bookingDetailsPage.GetQuickLinkJoinUrl(EnvConfigSettings.VideoUrl).Should().NotBeNullOrWhiteSpace();
        TestContext.WriteLine(bookingDetailsPage.GetQuickLinkJoinUrl(EnvConfigSettings.VideoUrl));
        dashboardPage.SignOut();
        Assert.Pass();
    }

    private async Task BookBasicHearing()
    {
        var date = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30);
        var request = HearingTestData.CreateNewRequestDtoWithOnlyAJudge(scheduledDateTime: date);
        _hearing = await BookingsApiClient.BookNewHearingAsync(request);
    }

    protected override async Task CleanUp()
    {
        if (_hearing != null)
        {
            TestContext.WriteLine($"Removing Hearing {_hearing.Id}");
            await BookingsApiClient.RemoveHearingAsync(_hearing.Id);
            _hearing = null;
        }
    }
}