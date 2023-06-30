using OpenQA.Selenium;

namespace UI.PageModels.Pages.Admin.Audio;

public class GetAudioFilePage : VhAdminWebPage
{
    private readonly By _hearingAudioFileRadio = By.CssSelector("label[for='search-choice-vhfile']");
    private readonly By _caseNumberInput = By.Id("caseNumber");
    private readonly By _searchButton = By.Id("submit");
    private readonly By _getLinkButton = By.XPath("//button[@id='getLinkButton']");
    private readonly By _getLinkErrorMessage = By.CssSelector("div[class='linkCopiedMessage vh-red'] label");
    private readonly By _copyLinkButton = By.XPath("//button[contains(@id,'copyLinkButton')]");
    private readonly By _linkCopiedMessage = By.XPath("//div[contains(@id,'linkCopied')]");

    public GetAudioFilePage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
        if (!Driver.Url.EndsWith("get-audio-file"))
            throw new InvalidOperationException(
                "This is not the GetAudioFileLink page, the current url is: " + Driver.Url);
    }
    
    public void EnterVideoHearingCaseDetailsAndCopyLink(string caseNumber)
    {
        ClickElement(_hearingAudioFileRadio);
        EnterText(_caseNumberInput, caseNumber);
        ClickElement(_searchButton);
        WaitForApiSpinnerToDisappear();
        
        ClickElement(_getLinkButton);
        WaitForApiSpinnerToDisappear();
        if (IsElementVisible(_getLinkErrorMessage))
        {
            throw new Exception($"There was an error getting the link for case number {caseNumber}. The linked error message appeared: {GetText(_getLinkErrorMessage)}");
        }
        ClickElement(_copyLinkButton);
        WaitForElementToBeVisible(_linkCopiedMessage);
    }
}