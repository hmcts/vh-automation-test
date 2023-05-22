using OpenQA.Selenium;

namespace UI.PageModels.Pages
{   
	///<summary>
	///   VHOHearingListPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class VHOHearingListPage
    {
        public static By HealingListRow => By.XPath("//tr[@class='govuk-table__row']");
        public static By ParticipantName => By.XPath("//*[contains(@Id,'participant-contact-details-link')]");  
        public static By ParticipantStatusInHearing => By.XPath("//p[contains(@Id,'participant-status-')][contains(text(),'In hearing')]");
        public static By ParticipantStatusJoining => By.XPath($"//p[contains(@Id,'participant-status-')][contains(text(),'Joining')]");
        public static By ParticipantStatusConnected=> By.XPath($"//p[contains(@Id,'participant-status-')][contains(text(),'Connected')]");
        public static By ParticipantStatusDisconnected => By.XPath($"//p[contains(@Id,'participant-status-')][contains(text(),'Disconnected')]");
        public static By ParticipantStatusNotSignedIn => By.XPath($"//p[contains(@Id,'participant-status-')][contains(text(),'Not signed in')]");
        public static By ParticipantStatusAvailable => By.XPath($"//p[contains(@Id,'participant-status-')][contains(text(),'Available')]");
        public static By ParticipantStatusInConsultation => By.XPath($"//p[contains(@Id,'participant-status-')][contains(text(),'In consultation')]");
        public static By ParticipantStatusUnavailable => By.XPath($"//p[contains(@Id,'participant-status-')][contains(text(),'Unavailable')]");
    }
}
