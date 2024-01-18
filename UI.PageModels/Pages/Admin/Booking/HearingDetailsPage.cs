namespace UI.PageModels.Pages.Admin.Booking;

public class HearingDetailsPage : VhAdminWebPage
{
    private readonly By _caseName = By.Id("caseName");
    private readonly By _caseNumber = By.Id("caseNumber");
    private readonly By _caseType = By.Id("caseType");
    private readonly By _hearingType = By.Id("hearingType");
    private readonly By _nextButton = By.Id("nextButton");

    public HearingDetailsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        if (!Driver.Url.EndsWith("book-hearing"))
            throw new InvalidOperationException(
                "This is not the HearingDetails page, the current url is: " + Driver.Url);
    }
    
    public void EnterHearingDetailsV2(string caseNumber, string caseName, string caseType)
    {
        WaitForApiSpinnerToDisappear();
        EnterText(_caseNumber, caseNumber);
        EnterText(_caseName, caseName);
        WaitForDropdownListToPopulate(_caseType);
        SelectDropDownByText(_caseType, caseType);
        
    }
    /// <summary>
    /// Enter the case details, case type and hearing type
    /// </summary>
    /// <param name="caseNumber">the case number, typically the property to search against</param>
    /// <param name="caseName">the case name</param>
    /// <param name="caseType">The name of the case type</param>
    /// <param name="hearingType">the name of the hearing type</param>
    public void EnterHearingDetails(string caseNumber, string caseName, string caseType, string hearingType)
    {
        WaitForApiSpinnerToDisappear();
        EnterText(_caseNumber, caseNumber);
        EnterText(_caseName, caseName);
        WaitForDropdownListToPopulate(_caseType);
        SelectDropDownByText(_caseType, caseType);
        SelectDropDownByText(_hearingType, hearingType);
    }
    
    /// <summary>
    /// Enter the details for a hearing and go to the summary page
    /// </summary>
    /// <param name="bookingDto">A DTO representing a booking</param>
    /// <returns>the summary page</returns>
    public SummaryPage EnterHearingDetails(BookingDto bookingDto, bool isV2)
    {
        if(isV2)
            EnterHearingDetailsV2(bookingDto.CaseNumber, bookingDto.CaseName, bookingDto.CaseType);
        else
            EnterHearingDetails(bookingDto.CaseNumber, bookingDto.CaseName, bookingDto.CaseType, bookingDto.HearingType);
        
        var hearingSchedulePage = GoToNextPage();

        hearingSchedulePage.EnterSingleDayHearingSchedule(bookingDto.ScheduledDateTime, bookingDto.DurationHour,
            bookingDto.DurationMinute, bookingDto.VenueName, bookingDto.RoomName);

        var assignJudgePage = hearingSchedulePage.GoToNextPage();

        if (isV2)
        {
            assignJudgePage.AssignPresidingJudiciaryDetails(bookingDto.Judge.Username, bookingDto.Judge.DisplayName);
            assignJudgePage.ClickSaveEJudgeButton();
        }
        else
            assignJudgePage.EnterJudgeDetails(bookingDto.Judge.Username, bookingDto.Judge.DisplayName, bookingDto.Judge.Phone);

        var addParticipantPage = assignJudgePage.GoToParticipantsPage(isV2);
        
        if(isV2)
            addParticipantPage.AddExistingParticipantsV2(bookingDto.Participants);
        else
            addParticipantPage.AddExistingParticipants(bookingDto.Participants);
        
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