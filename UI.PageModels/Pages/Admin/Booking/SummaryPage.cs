using OpenQA.Selenium;
using UI.PageModels.Dtos;

namespace UI.PageModels.Pages.Admin.Booking;

public class SummaryPage : VhAdminWebPage
{
    public static By ViewThisBooking = By.LinkText("View this booking");
    private readonly By _bookButton = By.Id("bookButton");
    private readonly By _dotLoader = By.Id("dot-loader");

    //private readonly By SuccessTitle = By.ClassName("govuk-panel__title");
    private readonly By _successTitle = By.XPath("//h1[text()[contains(.,'Your hearing booking was successful')]]");

    private readonly By _tryAgainButton = By.Id("btnTryAgain");

    public SummaryPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        WaitForElementToBeClickable(_bookButton);
        if (!Driver.Url.EndsWith("summary"))
            throw new InvalidOperationException(
                "This is not the summary page, the current url is: " + Driver.Url);
    }

    public void ValidateSummaryPage(BookingDto bookingDto)
    {
        CompareText(By.Id("caseNumber"), bookingDto.CaseNumber);
        CompareText(By.Id("caseName"), bookingDto.CaseName);
        CompareText(By.Id("caseType"), bookingDto.CaseType);
        CompareText(By.Id("caseHearingType"), bookingDto.HearingType);

        var courtAddress = string.IsNullOrWhiteSpace(bookingDto.RoomName)
            ? bookingDto.VenueName
            : $"{bookingDto.VenueName}, {bookingDto.RoomName}";
        CompareText(By.Id("courtAddress"), courtAddress);

        var hearingDate = $"{bookingDto.ScheduledDateTime:dddd dd MMMM yyyy, HH:mmtt}";
        CompareText(By.Id("hearingDate"), hearingDate);
        var duration =
            $"listed for {Pluralise(bookingDto.DurationHour, "hour")} {Pluralise(bookingDto.DurationMinute, "minute")}"
                .Trim();
        CompareText(By.Id("hearingDuration"), duration);
        
        CompareText(By.Id("audioRecording"), bookingDto.AudioRecording ? "Yes" : "No");

        for (var i = 0; i < bookingDto.VideoAccessPoints.Count-1; i++)
        {
            CompareText(By.XPath($"//div[normalize-space()='{bookingDto.VideoAccessPoints[i].DisplayName}']"),
                bookingDto.VideoAccessPoints[i].DisplayName);
        }

        if (!string.IsNullOrWhiteSpace(bookingDto.OtherInformation))
        {
            CompareText(By.Id("otherInformation"), bookingDto.OtherInformation);
        }
        
        // assert judge details
        CompareText(By.Id("judge-user"), bookingDto.Judge.Username);
        
        for (var i = 0; i < bookingDto.Participants.Count-1; i++)
        {
            var participant = bookingDto.Participants[i];
            var name = $"{participant.Title} {participant.FirstName} {participant.LastName}";
           
            CompareText(By.XPath($"//div[normalize-space()='{name}']"), name);
        }
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
        var text = GetText(element);
        if (!text.Equals(expectedText, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new Exception($"Expected text: {expectedText} but was {text}");
        }
    }

    public BookingConfirmationPage ClickBookButton()
    {
        ClickElement(_bookButton);
        WaitForElementToBeVisible(Spinner);
        WaitForApiSpinnerToDisappear(90); // booking process can take a while. lower when process has been optimised
        WaitForElementToBeVisible(_successTitle);
        return new BookingConfirmationPage(Driver, DefaultWaitTime);
    }
}