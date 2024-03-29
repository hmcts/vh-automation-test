﻿using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video.Participant;

public class DeclarationPage : VhVideoWebPage
{
    public DeclarationPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }
    
    protected override void ConfirmPageHasLoaded()
    {
        WaitForElementToBeClickable(DeclarationCheckBox);
    }

    public static By DeclarationCheckBox => By.CssSelector("label.govuk-label.govuk-checkboxes__label");
    public static By DeclarationContinueBtn => By.Id("nextButton");

    public ParticipantWaitingRoomPage AcceptDeclaration(bool isParticipantWithLimitedControls = false)
    {
        ConfirmPageHasLoaded();
        ClickElement(DeclarationCheckBox);
        ClickElement(DeclarationContinueBtn);
        return new ParticipantWaitingRoomPage(Driver, DefaultWaitTime, isParticipantWithLimitedControls);
    }
}