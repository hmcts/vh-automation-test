using TechTalk.SpecFlow;
using TestFramework;
using UI.Utilities;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Pages;
using UI.Utilities;
using System;
using OpenQA.Selenium.DevTools;

namespace UI.Steps;

[Binding]
public class WorkAllocationAllocateHearing :ObjectFactory

{
    private readonly ScenarioContext _scenarioContext;
    
    public WorkAllocationAllocateHearing(ScenarioContext scenarioContext)
        : base(scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    
    [Then(@"I click on Allocate Hearing")]
    public void ThenIClickOnAllocateHearing()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingsTab);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingsTab).Click();
    }

    [Then(@"I Select Date Range")]
    public void ThenISelectDateRange()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingsFromDate);
        var allocateHearingtoday = DateTime.Today.ToString("dd/MM/yyyy");
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingsFromDate).SendKeys(allocateHearingtoday);
    }

    [Then(@"I press Search")]
    public void ThenIPressSearch()
    {
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingSearchButton);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingSearchButton).Click();
    }

    [Then(@"I Select Allocate To User ""(.*)""")]
    public void ThenISelectAllocateToUser(string p0)
    {
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCSOSelectList).Click();
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingCSOSelectList); 
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCSOSelectList).Click();
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCSOSelectList).SendKeys("TESTUSERDONOT");
        ExtensionMethods.WaitForElementVisible(Driver, ManageWorkAllocationPage.AllocateHearingCsoSelect);
        Driver.FindElement(ManageWorkAllocationPage.AllocateHearingCsoSelect).Click();
        
    }
}