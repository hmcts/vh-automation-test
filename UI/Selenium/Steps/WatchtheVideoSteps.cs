using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Watch The Video
    ///</summary>
    public class WatchtheVideoSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;

        WatchtheVideoSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"I continue to watch the video")]
        public void ThenIContinueToWatchTheVideo()
        {
            ExtensionMethods.FindElementWithWait(Driver, WatchtheVideoPage.WatchVideoButton, _scenarioContext).Click();
        }        
    }
}