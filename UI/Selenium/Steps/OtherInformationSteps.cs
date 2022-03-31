using SeleniumSpecFlow.Utilities;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using UISelenium.Pages;
namespace UI.Steps
{
    public class OtherInformationSteps : ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;
        public OtherInformationSteps(ScenarioContext scenarioContext)
            :base (scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I set any other information")]
        public void GivenISetAnyOtherInformation(Table table)
        {
            _scenarioContext.UpdatePageName("Other information");
            SetAnyOtherHearingInfo(table);
            EnterOtherInformation();
        }

        private void SetAnyOtherHearingInfo(Table table)
        {
            _hearing = _scenarioContext.Get<Hearing>("Hearing");
            var tableRow = table.Rows[0];
            bool isRecorded = tableRow["Record Hearing"]?.ToLower() == "true"? true : false;
            var otherInformation = new OtherInformation
            {
                IsHearingRecorded = isRecorded,
                AnyOtherInfo = tableRow["Other information"]
            };
            _hearing.OtherInformation = otherInformation;
        }

        private void EnterOtherInformation()
        {
            ExtensionMethods.FindElementWithWait(Driver, OtherInformationPage.OtherInfo, _scenarioContext);
            if (_hearing.OtherInformation.IsHearingRecorded)
            {
                Driver.FindElement(OtherInformationPage.RecordAudioYes).Click();
            }
           
            if (!_hearing.OtherInformation.IsHearingRecorded && Driver.FindElement(OtherInformationPage.RecordAudioNo).Enabled)
            {
                var element = Driver.FindElement(OtherInformationPage.RecordAudioNo);
                element.Click();
            }

            if (!string.IsNullOrEmpty(_hearing.OtherInformation.AnyOtherInfo))
            {
                ExtensionMethods.FindElementWithWait(Driver, OtherInformationPage.OtherInfo, _scenarioContext).SendKeys(_hearing.OtherInformation.AnyOtherInfo);
            }

            ExtensionMethods.FindElementWithWait(Driver, OtherInformationPage.NextButton, _scenarioContext).Click();  
        }
    }
}
