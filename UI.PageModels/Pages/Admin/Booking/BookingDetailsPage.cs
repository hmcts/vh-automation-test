﻿using OpenQA.Selenium;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.Booking
{
    public class BookingDetailsPage : VhPage
    {
        public static By CancelBookingButton => By.Id("cancel-button");
        public static By EditBookingButton => By.Id("edit-button");
        public static By CloseBookingFailureWindowButton = By.Id("btnTryAgain");
        public static By BookingConfirmedStatus = By.XPath("//div[@class='vh-created-booking'][text()='Confirmed']");
        public static By SpecificBookingConfirmedStatus(string caseNumber) => By.XPath($"//div[@class='govuk-grid-column-full' and contains(.,'{caseNumber}') and contains(.,'Confirmed')]");
        public static By SpecificBookingCancelledStatus(string caseNumber) => By.XPath($"//div[@class='govuk-grid-column-full' and contains(.,'{caseNumber}') and contains(.,'Cancelled')]");
        public static By ConfirmCancelButton => By.Id("btnCancelBooking");
        public static By CancelReason => By.Id("cancel-reason");
        public static By ParticipantDetails => By.ClassName("participant-details");
        public static By CourtRoomAddress => By.Id("court-room-address");
        public static By HearingStart => By.Id("hearing-start");

        public BookingDetailsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
            WaitForElementToBeClickable(EditBookingButton);
            if (!Driver.Url.EndsWith("booking-details"))
            {
                throw new InvalidOperationException(
                    "This is not the booking-details page, the current url is: " + Driver.Url);
            }
        }
    }
}