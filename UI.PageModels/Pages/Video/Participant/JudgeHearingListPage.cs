using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant
{
    public  class JudgeHearingListPage : VhPage
    {
        private readonly By _checkEquipmentBtn = By.Id("check-equipment-btn");
        public static By HealingListRow => By.XPath("//tr[@class='govuk-table__row']");
        public static By SelectButton(string caseId) => By.XPath($"//tr[contains(.,'{caseId}')]//button");
        public static By ButtonNext => By.Id("next");
        public static By ContinueButton => By.Id("continue-btn");
        public static By SwitchOnButton => By.Id("switch-on-btn");
        public static By WatchVideoButton => By.Id("watch-video-btn");
        public static By CameraWorkingYes => By.Id("camera-yes");
        public static By MicrophoneWorkingYes => By.Id("microphone-yes");
        public static By VideoWorkingYes => By.Id("video-yes");
        public static By NextButton => By.Id("nextButton");
        public static By DeclareCheckbox => By.Id("declare");

        public JudgeHearingListPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
        {
            WaitForElementToBeClickable(_checkEquipmentBtn);
        }
        
        public JudgeWaitingRoomPage SelectHearing(string caseName)
        {
            var selectHearingLocator = By.XPath($"//tr[contains(.,'{caseName}')]//button");
            ClickElement(selectHearingLocator);
            return new JudgeWaitingRoomPage(Driver, DefaultWaitTime);
        }
    }
}
