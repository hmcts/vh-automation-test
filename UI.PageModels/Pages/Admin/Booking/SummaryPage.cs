using NUnit.Framework;

namespace UI.PageModels.Pages.Admin.Booking;

public class SummaryPage : VhAdminWebPage
{
    private readonly By _bookButton = By.Id("bookButton");
    private readonly By _addANoJudgeWarning = By.CssSelector("h2[id = govuk-notification-banner-title]");
   

    public SummaryPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(_bookButton);
        if (!Driver.Url.EndsWith("summary"))
            throw new InvalidOperationException(
                "This is not the summary page, the current url is: " + Driver.Url);
    }
    
    public void ValidateSummaryPage(BookingDto bookingDto, bool isMultiDay = false)
    {
        ValidateHearingDetails(bookingDto);
        ValidateVenueDetails(bookingDto);
        if (isMultiDay)
            ValidateDateMultiDay(bookingDto);
        else
            ValidateDateAndDuration(bookingDto);
        ValidateEndpointsAndOtherInformation(bookingDto);
        ValidateParticipantDetails(bookingDto);
    }

    public HearingAssignJudgePage ChangeJudgeV1()
    {
        ClickElement(By.XPath("//a[contains(@href, '/assign-judge')]"));
        return new HearingAssignJudgePage(Driver, DefaultWaitTime);
    }

    public HearingAssignJudgePage ChangeJudgeV2()
    {
        ClickElement(By.XPath("//a[contains(@href, '/add-judicial-office-holders')]"));
        return new HearingAssignJudgePage(Driver, DefaultWaitTime);
        
    } 
    
    private void ValidateEndpointsAndOtherInformation(BookingDto bookingDto)
    {
        for (var i = 0; i < bookingDto.VideoAccessPoints.Count-1; i++)
        {
            var endpoint = bookingDto.VideoAccessPoints[i];
            CompareText(By.Id($"displayName{i}"), endpoint.DisplayName);
        }

        if (!string.IsNullOrWhiteSpace(bookingDto.OtherInformation))
        {
            CompareText(By.Id("otherInformation"), bookingDto.OtherInformation);
        }
    }

    private void ValidateParticipantDetails(BookingDto bookingDto)
    {
        CompareText(By.Id("judge-user"), bookingDto.Judge.Username);

        var allParticipants = bookingDto.Participants.Concat(bookingDto.NewParticipants).ToList();
        for (var i = 0; i < allParticipants.Count-1; i++)
        {
            var participant = allParticipants[i];
            var name = $"{participant.Title} {participant.FirstName} {participant.LastName}";
           
            CompareText(By.XPath($"//div[normalize-space()='{name}']"), name);
        }
    }

    private void ValidateDateAndDuration(BookingDto bookingDto)
    {
        var hearingDate = $"{bookingDto.ScheduledDateTime:dddd dd MMMM yyyy, HH:mmtt}";
        CompareText(By.Id("hearingDate"), hearingDate);
        var duration =
            $"listed for {Pluralise(bookingDto.DurationHour, "hour")} {Pluralise(bookingDto.DurationMinute, "minute")}"
                .Trim();
        CompareText(By.Id("hearingDuration"), duration);
    }
    
    private void ValidateDateMultiDay(BookingDto bookingDto)
    {
        var startDate = $"{bookingDto.ScheduledDateTime:dddd dd MMMM yyyy} -";
        var endDate = $"{bookingDto.EndDateTime:dddd dd MMMM yyyy, H:mmtt}";
        
        CompareText(By.Id("hearingStartDate"), startDate);
        CompareText(By.Id("hearingEndDateTime"), endDate);
    }
    
    private void ValidateVenueDetails(BookingDto bookingDto)
    {
        var courtAddress = string.IsNullOrWhiteSpace(bookingDto.RoomName)
            ? bookingDto.VenueName
            : $"{bookingDto.VenueName}, {bookingDto.RoomName}";
        CompareText(By.Id("courtAddress"), courtAddress);
    }

    private void ValidateHearingDetails(BookingDto bookingDto)
    {
        CompareText(By.Id("caseNumber"), bookingDto.CaseNumber);
        CompareText(By.Id("caseName"), bookingDto.CaseName);
        CompareText(By.Id("caseType"), bookingDto.CaseType);
        CompareText(By.Id("audioRecording"), bookingDto.AudioRecording ? "Yes" : "No");
    }
    
    public void ValidateSummaryNoJudgePage(BookingDto bookingDto)
    {
        CompareText(By.Id("caseNumber"), bookingDto.CaseNumber);
        CompareText(By.Id("caseName"), bookingDto.CaseName);
        CompareText(By.Id("caseType"), bookingDto.CaseType);

        var courtAddress = string.IsNullOrWhiteSpace(bookingDto.RoomName)
            ? bookingDto.VenueName
            : $"{bookingDto.VenueName}, {bookingDto.RoomName}";
        CompareText(By.Id("courtAddress"), courtAddress);

        var hearingDate = $"{bookingDto.ScheduledDateTime:dddd dd MMMM yyyy, HH:mmtt}";
        CompareText(By.Id("hearingDate"), hearingDate);
        var duration = $"listed for {Pluralise(bookingDto.DurationHour, "hour")} {Pluralise(bookingDto.DurationMinute, "minute")}".Trim();
        CompareText(By.Id("hearingDuration"), duration);
        
        CompareText(By.Id("audioRecording"), bookingDto.AudioRecording ? "Yes" : "No");

        for (var i = 0; i < bookingDto.VideoAccessPoints.Count-1; i++)
        {
            var endpoint = bookingDto.VideoAccessPoints[i];
            CompareText(By.Id($"displayName{i}"), endpoint.DisplayName);
        }

        if (!string.IsNullOrWhiteSpace(bookingDto.OtherInformation))
        {
            CompareText(By.Id("otherInformation"), bookingDto.OtherInformation);
        }
        
        Assert.AreEqual("Important", Driver.FindElement(_addANoJudgeWarning).Text);
    }
    
    private string Pluralise(int number, string text)
    {
        if (number == 0)
        {
            return string.Empty;
        }
        
        return number > 1 ? $"{number} {text}s" : $"{number} {text}";
    }

    private void CompareText(By element, string expectedText)
    {
        var text = GetText(element).Trim();
        if (!text.Equals(expectedText.Trim(), StringComparison.InvariantCultureIgnoreCase))
        {
            throw new InvalidOperationException($"Expected text: {expectedText} but was {text}");
        }
    }

    public BookingConfirmationPage ClickBookButton()
    {
        ClickElement(_bookButton);
        WaitForApiSpinnerToDisappear(90); // booking process can take a while. lower when process has been optimised
        WaitForElementToBeVisible(By.TagName("app-booking-confirmation"));
        return new BookingConfirmationPage(Driver, DefaultWaitTime);
    }
}