﻿using OpenQA.Selenium;

namespace UI.PageModels.Pages.Video
{
	///<summary>
	///   DeclarationPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class DeclarationPage
    {
        public static By DeclarationCheckBox => By.CssSelector("label.govuk-label.govuk-checkboxes__label");
        public static By DeclarationContinueBtn => By.Id("nextButton");
    }
}