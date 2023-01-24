using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Testing equipment page
    ///</summary>
    public class TestingEquipmentSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;
        TestingEquipmentSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"Testing your equipment")]
        public void ThenIConfirmEquipmentIsWorking()
        {
            ExtensionMethods.FindElementWithWait(Driver, TestingEquipmentPage.ContinueBtn, _scenarioContext).Click();
        }      
    }
}