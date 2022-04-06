using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
namespace UISelenium.Pages
{
    public class HearingListPage
    {
        public static By ConferenceList = By.ClassName("conference-list");
        public static By VenueListItem(string hearingId) => By.XPath($"//div[@id='{hearingId}-venue']");
        public static By SummaryListItem(string hearingId) => By.XPath($"//div[@id='{hearingId}-summary']");
        public static By CaseNumberListItem(string hearingId) => By.XPath($"//div[@id='{hearingId}-case-number']");
        public static By CaseNameListItem(string hearingId) => By.XPath($"//div[@id='{hearingId}-case-name']");
        public static By Quicklinks => By.CssSelector("fa-icon");
        public static By CloseButton => By.Id("closeButton");
    }
}
