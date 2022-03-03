using OpenQA.Selenium;

namespace UISelenium.Pages
{
    public class IdpSelectionPage
    {
        public static By JudicialOfficeHolder = By.XPath("//label[contains(.,'Judicial')]");
        public static By HearingParticipant = By.XPath("//label[contains(.,'Participant')]");
        public static By NextButton = By.TagName("button");
    }
}
