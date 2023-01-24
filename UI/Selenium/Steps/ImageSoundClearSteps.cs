using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using TestFramework;
using OpenQA.Selenium.Interactions;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Image/Sound Clear page
    ///</summary>
    public class ImageSoundClearSteps : ObjectFactory
    {
        ScenarioContext _scenarioContext;

        ImageSoundClearSteps(ScenarioContext scenarioContext)
            : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"Checking were the image and sound clear")]
        public void ThenCheckingWereTheImageAndSoundClear()
        {
            ExtensionMethods.FindElementWithWait(Driver, ImageSoundClearPage.VideoYesRadioButton, _scenarioContext).Click();
            ExtensionMethods.FindElementWithWait(Driver, ImageSoundClearPage.Continue, _scenarioContext).Click();
        }
    }
}