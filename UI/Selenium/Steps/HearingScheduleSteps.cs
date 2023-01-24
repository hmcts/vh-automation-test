﻿using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;
using TestFramework;
using UI.Model;
using System.Linq;
using UI.Pages;
using UI.Utilities;

namespace UI.Steps
{
    [Binding]
    ///<summary>
    /// Steps class for Hearing Schedule Page
    ///</summary>
    public class HearingScheduleSteps :ObjectFactory
    {
        private readonly ScenarioContext _scenarioContext;
        private Hearing _hearing;
        
        public HearingScheduleSteps(ScenarioContext scenarioContext)
            :base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _hearing = (Hearing)_scenarioContext["Hearing"];
        }

        [Given(@"the hearing has the following schedule details")]
        public void GivenTheHearingHasTheFollowingScheduleDetails(Table table)
        {
            _scenarioContext.UpdatePageName("Hearing schedule");
            _hearing = CreateHearingModel(table);
            EnterHearingSchedule(_hearing.HearingSchedule);
        }

        public void GivenTheHearingHasTheScheduleDetails(Table table, int min = 3)
        {
            _scenarioContext.UpdatePageName("Hearing schedule");
            _hearing = CreateHearingModel(table, min);
            EnterHearingSchedule(_hearing.HearingSchedule);
        }

        private void EnterHearingSchedule(HearingSchedule hearingSchedule)
        {
            ExtensionMethods.FindElementEnabledWithWait(Driver, HearingSchedulePage.HearingDate).SendKeys(hearingSchedule.HearingDate.FirstOrDefault().ToString("dd/MM/yyyy"));
            Driver.FindElement(HearingSchedulePage.HearingDate).Click();
            Driver.FindElement(HearingSchedulePage.HearingStartTimeHour).SendKeys(hearingSchedule.HearingDate.FirstOrDefault().ToString("HH"));
            Driver.FindElement(HearingSchedulePage.HearingStartTimeMinute).SendKeys(hearingSchedule.HearingDate.FirstOrDefault().ToString("mm"));
            Driver.FindElement(HearingSchedulePage.HearingDurationHour).SendKeys(hearingSchedule.DurationHours);
            Driver.FindElement(HearingSchedulePage.HearingDurationMinute).SendKeys(hearingSchedule.DurationMinutes);
            new SelectElement(Driver.FindElement(HearingSchedulePage.CourtVenue)).SelectByText(hearingSchedule.HearingVenue);
            Driver.FindElement(HearingSchedulePage.CourtRoom).SendKeys(hearingSchedule.HearingRoom);
            Driver.FindElement(HearingDetailsPage.NextButton).Click();
        }

        public Hearing CreateHearingModel(Table table, int min = 10)
        {
            var tableRow = table.Rows[0];
            var date = DateTime.Now.AddMinutes(min);
             _hearing.HearingSchedule.HearingDate = new System.Collections.Generic.List<DateTime> { date };
            _hearing.HearingSchedule.HearingTime = date;
            _hearing.HearingSchedule.DurationHours = tableRow["Duration Hour"];
            _hearing.HearingSchedule.DurationMinutes = tableRow["Duration Minute"];
            _hearing.HearingSchedule.HearingVenue = "Birmingham Civil and Family Justice Centre";
            _hearing.HearingSchedule.HearingRoom = new Random().Next(0, 9).ToString();
            _scenarioContext["Hearing"] = _hearing;
            return _hearing;
        }
    }
}
