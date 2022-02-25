using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
namespace UISelenium.Pages
{
    public class ParticipantWaitingRoomPage
    {
        public static By StartVideoHearingButton => By.XPath("//button[contains(text(),'Start video hearing')]");
        public static By EnterConsultationRoomButton => By.Id("joinPCButton");
        public static By ConfirmStartButton => By.Id("btnConfirmStart");
        public static By CancelStartHearingButton => By.Id("btnCancelStart");
        public static By HearingClosedTitle => By.XPath("//h1[contains(text(),'This hearing has finished. You may now sign out')]");
        public static By ParticipantDetails(string name) => By.XPath($"//*[contains(text(),'{name}')]");
        public static By ChooseCameraAndMicButton => By.Id("changeCameraButton");
        public static By JoinPrivateMeetingButton => By.Id("changeCameraButton");
        public static string ParticipantWaitingRoomClosedTitle = "This hearing has finished. You may now sign out";
    }
}
