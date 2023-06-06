using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
    public class DeclarationPage : VhVideoWebPage
    {
        public static By DeclarationCheckBox => By.CssSelector("label.govuk-label.govuk-checkboxes__label");
        public static By DeclarationContinueBtn => By.Id("nextButton");

        public DeclarationPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
        }
        
        public ParticipantWaitingRoomPage AcceptDeclaration()
        {
            ClickElement(DeclarationCheckBox);
            ClickElement(DeclarationContinueBtn);
            return new ParticipantWaitingRoomPage(Driver, DefaultWaitTime);
        }
    }
}