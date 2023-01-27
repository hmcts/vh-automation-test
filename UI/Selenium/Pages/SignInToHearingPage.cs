using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   ConfirmEquipmentPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class SignInToHearingPage
    {
        public static By CheckEquipmentButton => By.Id("check-equipment-btn");
        public static By SignInToHearingButton => By.XPath("//button[contains(@id,'sign-into-hearing-btn-')]");
    }
}