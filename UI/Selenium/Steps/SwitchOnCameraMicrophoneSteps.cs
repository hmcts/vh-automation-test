using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Switch On Camera/Microphone page
    ///</summary>
    public class SwitchOnCameraMicrophoneSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;

        SwitchOnCameraMicrophoneSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"I make sure camera and microphone switched on")]
        public void ThenIMakeSureCameraAndMicrophoneSwitchedOn()
        {
            ExtensionMethods.FindElementWithWait(Driver, SwitchOnCameraMicrophonePage.SwitchOnButton, _scenarioContext).Click();
        }
    }
}