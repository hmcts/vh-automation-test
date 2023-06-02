using OpenQA.Selenium;
using UI.PageModels.Dtos;
using UI.PageModels.Pages.Video;

namespace UI.PageModels.Pages.Admin.Booking
{
	public class ParticipantsPage : VhAdminWebPage
	{
		private readonly By _partyDropdown = By.Id("party");
		private readonly By _roleDropdown = By.Id("role");
		private readonly By _participantEmailTextfield = By.Id("participantEmail");
		private readonly By _titleDropdown = By.Id("title");
		private readonly By _firstNameTextfield = By.Id("firstName");
		private readonly By _lastNameTextfield = By.Id("lastName");
		private readonly By _individualOrganisationTextfield = By.Id("companyNameIndividual");
		private readonly By _repOrganisationTextfield = By.Id("companyName");
		private readonly string _repOrganisationTextfieldId = "companyName";
		private readonly By _phoneTextfield = By.Id("phone");
		private readonly By _interpreterFor = By.Id("interpreterFor");
		private readonly By _displayNameTextfield = By.Id("displayName");
		private readonly By _representingTextfield = By.Id("representing");
		private readonly By _addParticipantLink = By.Id("addParticipantBtn");
		private readonly By _updateParticipantLink = By.Id("updateParticipantBtn");
		private readonly By _clearDetailsLink = By.PartialLinkText("Clear details");
		private readonly By _nextButton = By.Id(("nextButton"));
		private readonly By _cancelButton = By.Id("cancelBtn");
		private readonly By _existingEmailLinks = By.XPath("//li[@class='vk-showlist-m30']/a");
		private readonly By _participantsList = By.XPath("//*[contains(@class, 'vhtable-header')]");
		private readonly By _interpreteeDropdown = By.Id("interpreterFor");
		private readonly By _emailList = By.XPath("//ul[@id='search-results-list']//li");

		private readonly By _newUserWarning =
			By.XPath("//*[@id='search-email-component']/app-search-email/div/div[2]/strong");

		public ParticipantsPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
		{
			WaitForApiSpinnerToDisappear();
			WaitForElementToBeClickable(_nextButton);
			WaitForDropdownListToPopulate(_partyDropdown);
		}

		public void AddNewIndividualParticipant(string party, string role, string contactEmail, string displayName,
			NewParticipantDto newParticipantDto)
		{
			SelectDropDownByText(_partyDropdown, party);
			SelectDropDownByText(_roleDropdown, role);
			EnterText(_participantEmailTextfield, contactEmail);
			WaitForElementToBeVisible(_newUserWarning);

			SelectDropDownByText(_titleDropdown, newParticipantDto.Title);
			EnterText(_firstNameTextfield, newParticipantDto.FirstName);
			EnterText(_lastNameTextfield, newParticipantDto.LastName);
			EnterText(_individualOrganisationTextfield, newParticipantDto.Organisation);
			EnterText(_phoneTextfield, newParticipantDto.Telephone);
			EnterText(_displayNameTextfield, displayName);
		}

		public void AddNewRepresentative(string party, string role, string contactEmail, string displayName,
			string representing, NewParticipantDto newParticipantDto)
		{
			SelectDropDownByText(_partyDropdown, party);
			SelectDropDownByText(_roleDropdown, role);
			EnterText(_participantEmailTextfield, contactEmail);
			WaitForElementToBeVisible(_newUserWarning);

			SelectDropDownByText(_titleDropdown, newParticipantDto.Title);
			EnterText(_firstNameTextfield, newParticipantDto.FirstName);
			EnterText(_lastNameTextfield, newParticipantDto.LastName);
			EnterText(_individualOrganisationTextfield, newParticipantDto.Organisation);
			EnterText(_phoneTextfield, newParticipantDto.Telephone);
			EnterText(_displayNameTextfield, displayName);
		}

		private void AddNewParticipant(string party, string role, string contactEmail, string displayName,
			NewParticipantDto newParticipantDto, string? representing = null)
		{
			SelectDropDownByText(_partyDropdown, party);
			SelectDropDownByText(_roleDropdown, role);
			EnterText(_participantEmailTextfield, contactEmail);
			WaitForElementToBeVisible(_newUserWarning);

			SelectDropDownByText(_titleDropdown, newParticipantDto.Title);
			EnterText(_firstNameTextfield, newParticipantDto.FirstName);
			EnterText(_lastNameTextfield, newParticipantDto.LastName);
			EnterText(_individualOrganisationTextfield, newParticipantDto.Organisation);
			EnterText(_phoneTextfield, newParticipantDto.Telephone);
			EnterText(_displayNameTextfield, displayName);

			if (!string.IsNullOrWhiteSpace(representing))
			{
				EnterText(_representingTextfield, representing);
			}

			ClickAddParticipantAndWait();
		}

		public void AddExistingIndividualParticipant(string party, string role, string contactEmail, string displayName)
		{
			AddExistingParticipant(party, role, contactEmail, displayName);
		}

		public void AddExistingRepresentative(string party, string role, string contactEmail, string displayName,
			string representing)
		{
			AddExistingParticipant(party, role, contactEmail, displayName, representing);
		}

		private void AddExistingParticipant(string party, string role, string contactEmail, string displayName,
			string? representing = null)
		{
			SelectDropDownByText(_partyDropdown, party);
			SelectDropDownByText(_roleDropdown, role);
			EnterText(_participantEmailTextfield, contactEmail);

			ClickElement(_emailList);
			EnterText(_displayNameTextfield, displayName);

			if (!string.IsNullOrWhiteSpace(representing))
			{
				EnterText(_representingTextfield, representing);
			}

			ClickAddParticipantAndWait();
		}

		private void ClickAddParticipantAndWait()
		{
			Thread.Sleep(
				500); // THIS IS ABSOLUTELY REQUIRED - the component takes 500ms to respond to change based on a setTimeout
			ClickElement(_addParticipantLink);
			WaitForElementToBeInvisible(_addParticipantLink, 5);
			Thread.Sleep(
				500); // THIS IS ABSOLUTELY REQUIRED - the component takes 500ms to respond to change based on a setTimeout
		}

		public VideoAccessPointsPage GoToNextPage()
		{
			ClickElement(_nextButton);
			return new VideoAccessPointsPage(Driver, DefaultWaitTime);
		}
	}
}
