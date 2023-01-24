using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using OpenQA.Selenium.Support.UI;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Video Access
    ///</summary>
    public class VideoAccessSteps : ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;
        public VideoAccessSteps(ScenarioContext scenarioContext)
            :base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"With video Access points details")]
        public void GivenWithVideoAccessPointsDetails(Table table)
        {
            _scenarioContext.UpdatePageName("Video access points");
            SetVideoAccessPoints(table);
            EnterVideoAcessPoints();
        }

        private void SetVideoAccessPoints(Table table)
        {
            _hearing = _scenarioContext.Get<Hearing>("Hearing");
            foreach (var row in table.Rows)
            {
                var videoAccessPoints = new VideoAccessPoints
                {
                    DisplayName = row["Display Name"],
                    Advocate = row["Advocate"]
                };
                _hearing.VideoAccessPoints.Add(videoAccessPoints);
            }
        }

        private void EnterVideoAcessPoints()
        {
            int i = 0;
            ExtensionMethods.GetSelectElementWithText(Driver, VideoAccessPointsPage.DefenceAdvocate(i), "None", _scenarioContext);
            foreach (var accessPoints in _hearing.VideoAccessPoints)
            {
                if(!string.IsNullOrEmpty(accessPoints.DisplayName))
                    ExtensionMethods.FindElementWithWait(Driver, VideoAccessPointsPage.DisplayName(i), _scenarioContext).SendKeys(accessPoints.DisplayName);
                if (!string.IsNullOrEmpty(accessPoints.Advocate))
                    new SelectElement(ExtensionMethods.FindElementWithWait(Driver, VideoAccessPointsPage.DefenceAdvocate(i), _scenarioContext)).SelectByText(accessPoints.Advocate);
                i++;
                ExtensionMethods.FindElementWithWait(Driver, VideoAccessPointsPage.AddAnotherBtn, _scenarioContext).Click();
            }
            ExtensionMethods.FindElementWithWait(Driver, VideoAccessPointsPage.NextButton, _scenarioContext).Click();
        }
    }
}
