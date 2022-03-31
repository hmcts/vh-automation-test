using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
namespace UISelenium.Pages
{
    public class BookingListPage
    {
        public static By VideoHearingsTable => By.Id("vh-table");
        public static By HearingDateTitle => By.XPath($"//div[text()[contains(.,'{DateTime.Today.ToString("dd MMMM yyyy")}')]]");
        public static By HearingDetailsRow => By.XPath("//div[@class='vh-row-created']//div[@class='govuk-grid-row vh-row vh-a']");
        public static By HearingDetailsRowSpecific(string caseNumber) => By.XPath($"//div[text()[contains(.,'{caseNumber}')]]");
        public static By HearingSelectionSpecificRow(string caseNumber) => By.XPath($"//div[@class='govuk-grid-row vh-row vh-a' and contains(.,'{caseNumber}')]//div[@class='vh-created-booking']");
        public static By SearchCaseTextBox = By.Id("caseNumber");
        public static By SearchButton => By.Id("searchButton");
        public static By ConfirmedButton => By.XPath("//*[contains(text(),'Confirmed')]");
        public static By TelephoneParticipantLink => By.XPath("//div[@id='conference_phone_details']");
        public static By VideoParticipantLink => By.XPath("//div[contains(text(),'video-participant-link')]");
        public static By SearchPanelButton => By.Id("openSearchPanelButton");
    }
}