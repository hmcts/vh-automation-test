using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   GetAudioFilePage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class GetAudioFilePage
    {
        public static By HearingAudioFileRadio => By.Id("search-choice-vhfile");
        public static By CvpAudioFileRadio => By.Id("search-choice-cvpfile");
        public static By CaseNumberInput => By.Id("caseNumber");
        public static By SearchButton => By.Id("submit");
        public static By GetLinkButton => By.XPath($"//button[@id='getLinkButton']");
        public static By CopyLinkButton => By.XPath($"//button[contains(@id,'copyLinkButton')]");
        public static By LinkCopiedMessage => By.XPath("//div[contains(@id,'linkCopied')]");
    }
}
