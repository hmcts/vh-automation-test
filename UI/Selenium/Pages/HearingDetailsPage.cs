using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   HearingDetailsPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    internal class HearingDetailsPage
    {
        public static By CaseNumber = By.Id("caseNumber");
        public static By CaseName = By.Id("caseName");
        public static By CaseType = By.Id("caseType");
        public static By HeardingType = By.Id("hearingType");
        public static By NextButton = By.Id("nextButton");
    }
}
