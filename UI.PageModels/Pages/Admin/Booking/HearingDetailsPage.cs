namespace UI.PageModels.Pages.Admin.Booking;

public class HearingDetailsPage : VhAdminWebPage
{
    private readonly By _caseName = By.Id("caseName");
    private readonly By _caseNumber = By.Id("caseNumber");
    private readonly By _caseType = By.Id("caseType");
    private readonly By _nextButton = By.Id("nextButton");

    public HearingDetailsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        if (!Driver.Url.EndsWith("book-hearing"))
            throw new InvalidOperationException(
                "This is not the HearingDetails page, the current url is: " + Driver.Url);
    }

    public void EnterHearingDetails(BookingDto bookingDto)
    {
        EnterHearingDetailsV2(bookingDto.CaseNumber, bookingDto.CaseName, bookingDto.CaseType);

    }

    private void EnterHearingDetailsV2(string caseNumber, string caseName, string caseType)
    {
        WaitForApiSpinnerToDisappear();
        EnterText(_caseNumber, caseNumber);
        EnterText(_caseName, caseName);
        WaitForDropdownListToPopulate(_caseType);
        SelectDropDownByText(_caseType, caseType);
        
    }

    /// <summary>
    /// Enter the details for a hearing and go to the summary page
    /// </summary>
    /// <param name="bookingDto">A DTO representing a booking</param>
    /// <param name="isMultiDay">Is this a multi-day booking</param>
    /// <returns>the summary page</returns>
    public SummaryPage BookAHearingJourney(BookingDto bookingDto, bool isMultiDay = false)
    {
            EnterHearingDetailsV2(bookingDto.CaseNumber, bookingDto.CaseName, bookingDto.CaseType);
        
        
        var hearingSchedulePage = GoToNextPage();

        if (isMultiDay)
        {
            hearingSchedulePage.EnterMultiDayHearingSchedule(bookingDto);
        }
        else
        {
            hearingSchedulePage.EnterSingleDayHearingSchedule(bookingDto);
        }

        var assignJudgePage = hearingSchedulePage.GoToNextPage();
        assignJudgePage.EnterJudgeDetails(bookingDto.Judge);

        var addParticipantPage = assignJudgePage.GotToNextPage();
        
        addParticipantPage.AddParticipants(bookingDto.Participants);
        
        var videoAccessPointsPage = addParticipantPage.GoToVideoAccessPointsPage();
        videoAccessPointsPage.AddVideoAccessPoints(bookingDto.VideoAccessPoints);
        var otherInformationPage = videoAccessPointsPage.GoToOtherInformationPage(); 
        otherInformationPage.TurnOffAudioRecording();
        otherInformationPage.EnterOtherInformation(bookingDto.OtherInformation);

        return otherInformationPage.GoToSummaryPage();
    }

    /// <summary>
    /// Go to the next page or the booking journey, the hearing schedule page
    /// </summary>
    /// <returns></returns>
    public HearingSchedulePage GoToNextPage()
    {
        ClickElement(_nextButton);
        return new HearingSchedulePage(Driver, DefaultWaitTime);
    }

}