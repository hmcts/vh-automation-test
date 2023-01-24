using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   HearingSchedulePage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class HearingSchedulePage
    {
        public static By MultiDaysHearing = By.Id("multiDaysHearing");
        public static By HearingDate = By.Id("hearingDate");
        public static By HearingStartTimeHour = By.Id("hearingStartTimeHour");
        public static By HearingStartTimeMinute = By.Id("hearingStartTimeMinute");
        public static By CourtVenue = By.Id("courtAddress");
        public static By CourtRoom = By.Id("court-room");
        public static By HearingDurationHour = By.Id("hearingDurationHour");
        public static By HearingDurationMinute = By.Id("hearingDurationMinute");
    }
}
