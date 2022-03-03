using OpenQA.Selenium.Support.UI;
using SeleniumSpecFlow.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UISelenium.Pages;
using TestLibrary.Utilities;
using SeleniumExtras.WaitHelpers;

namespace UI.Steps
{
    [Binding]
    public class ParticipantsSteps : ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;
        public ParticipantsSteps (ScenarioContext context)
            :base(context)
        {
            _scenarioContext = context;
        }

        [Given(@"I want to create a Hearing for")]
        public void GivenIWantToCreateAHearingFor(Table table)
        {
            _scenarioContext.UpdatePageName("Add a participant");
            SetHearingParticipants(table);
            EnterParticipants();
        }


        private void SetHearingParticipants(Table table)
        {
            if (_scenarioContext.ContainsKey("Hearing"))
            {
                _hearing = _scenarioContext.Get<Hearing>("Hearing");
            }
            else
            {
                _hearing = new Hearing();
                _scenarioContext.Add("Hearing", _hearing);
            }
 
            foreach (var row in table.Rows)
            {
                var participant = new Participant
                {
                    Party = new Party
                    {
                        Name = row["Party"]
                    },
                    Role = new Role
                    {
                        Name = row["Role"]
                    },
                    Id = row["Id"],
                    Name = new Name
                    {
                        FirstName = $"AutoFirst{Util.RandomAlphabet(4)}",
                        LastName = $"AutoLast{Util.RandomAlphabet(4)}"
                    },
                    VideoOff=row.ContainsKey("Video Off") ? bool.Parse(row["Video Off"]) : false
                };

                _hearing.Participant.Add(participant);
            }

            _scenarioContext["Hearing"] = _hearing;
        }

        public void EnterParticipants()
        {
            
            foreach (var participant in _hearing.Participant)
            {
                if (!string.IsNullOrEmpty(participant.Party?.Name) && participant.Party?.Name != "Judge")
                {
                    WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(int.Parse(Config.DefaultElementWait)));
                    ExtensionMethods.GetSelectElementWithText(Driver, ParticipantsPage.PartyDropdown, participant.Party.Name, _scenarioContext);
                    _scenarioContext.UpdateElementName("PartyDropdown");
                    _scenarioContext.UpdateActionName("SendKeys");
                    new SelectElement(ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.PartyDropdown, _scenarioContext)).SelectByText(participant.Party.Name);
                    _scenarioContext.UpdateElementName("RoleDropdown");
                    _scenarioContext.UpdateActionName("SendKeys");
                    new SelectElement(ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.RoleDropdown, _scenarioContext)).SelectByText(participant.Role.Name);
                    _scenarioContext.UpdateElementName("ParticipantEmailTextfield");
                    _scenarioContext.UpdateActionName("SendKeys");
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.ParticipantEmailTextfield, _scenarioContext).SendKeys(participant.Id.Replace("hearings.reform.hmcts.net", "hmcts.net"));
                    _scenarioContext["Hearing"] = _hearing;
                    _scenarioContext.UpdateElementName("FirstNameTextfield");
                    _scenarioContext.UpdateActionName("SendKeys");
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.FirstNameTextfield, _scenarioContext).SendKeys(participant.Name.FirstName);
                    _scenarioContext.UpdateElementName("LastNameTextfield");
                    _scenarioContext.UpdateActionName("SendKeys");
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.LastNameTextfield, _scenarioContext).SendKeys(participant.Name.LastName);
                    _scenarioContext.UpdateElementName("PhoneTextfield");
                    _scenarioContext.UpdateActionName("SendKeys");
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.PhoneTextfield, _scenarioContext).SendKeys("07021234567");
                    if (ExtensionMethods.IsElementVisible(Driver, ParticipantsPage.RepresentingTextfield,_scenarioContext))
                    {
                        _scenarioContext.UpdateElementName("RepresentingTextfield");
                        _scenarioContext.UpdateActionName("SendKeys");
                        ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.RepresentingTextfield, _scenarioContext).SendKeys($"AutoRepresent{Util.RandomAlphabet(4)}");
                        _scenarioContext.UpdateElementName("RepOrganisationTextfield");
                        _scenarioContext.UpdateActionName("SendKeys");
                        ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.RepOrganisationTextfield, _scenarioContext).SendKeys($"AutoOrg{Util.RandomAlphabet(4)}");
                    }
                    _scenarioContext.UpdateElementName("DisplayNameTextfield");
                    _scenarioContext.UpdateActionName("SendKeys");
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.DisplayNameTextfield, _scenarioContext).SendKeys($"{participant.Name.FirstName} {participant.Name.LastName}");
                    _scenarioContext.UpdateElementName("AddParticipantLink");
                    _scenarioContext.UpdateActionName("SendKeys");
                    ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.AddParticipantLink, _scenarioContext).Click();
                }
            }
            _scenarioContext.UpdateElementName("ParticipantsPage.NextButton");
            _scenarioContext.UpdateActionName("Click");
            ExtensionMethods.FindElementWithWait(Driver, ParticipantsPage.NextButton, _scenarioContext).Click();
        }
    }
}
