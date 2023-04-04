using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   ParticipantsPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class ParticipantsPage
    {
        public static By PartyDropdown = By.Id("party");
        public static By RoleDropdown = By.Id("role");
        public static By ParticipantEmailTextfield = By.Id("participantEmail");
        public static By TitleDropdown = By.Id("title");
        public static By FirstNameTextfield = By.Id("firstName");
        public static By LastNameTextfield = By.Id("lastName");
        public static By IndividualOrganisationTextfield = By.Id("companyNameIndividual");
        public static By RepOrganisationTextfield = By.Id("companyName");
        public static string RepOrganisationTextfieldId = "companyName";
        public static By PhoneTextfield = By.Id("phone");
        public static By InterpreterFor = By.Id("interpreterFor");
        public static By DisplayNameTextfield = By.Id("displayName");
        public static By RepresentingTextfield = By.Id("representing");
        public static By AddParticipantLink = By.Id("addParticipantBtn");
        public static By UpdateParticipantLink = By.Id("updateParticipantBtn");
        public static By ClearDetailsLink = By.PartialLinkText("Clear details");
        public static By NextButton = By.Id(("nextButton"));
        public static By CancelButton = By.Id("cancelBtn");
        public static By ExistingEmailLinks = By.XPath("//li[@class='vk-showlist-m30']/a");
        public static By ParticipantsList = By.XPath("//*[contains(@class, 'vhtable-header')]");
        public static By InterpreteeDropdown = By.Id("interpreterFor");
        public static By EmailList = By.XPath("//ul[@id='search-results-list']//li");
    }
}
