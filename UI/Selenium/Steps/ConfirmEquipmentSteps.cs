using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Confirm Equipment page
    ///</summary>
    public class ConfirmEquipmentSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;

        ConfirmEquipmentSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"I confirm equipment is working")]
        public void ThenIConfirmEquipmentIsWorking()
        {
            ExtensionMethods.FindElementWithWait(Driver, ConfirmEquipmentPage.ContinueBtn, _scenarioContext).Click();
        }
    }
}