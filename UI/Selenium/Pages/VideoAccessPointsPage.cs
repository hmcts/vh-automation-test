using OpenQA.Selenium;

namespace UI.Pages
{
	///<summary>
	///   VideoAccessPointsPage
	///   Page element definitions
	///   Do not add logic here
	///</summary>
    public class VideoAccessPointsPage
    {
        public static By DisplayName(int number) => By.Id($"displayName{number}");
        public static By DefenceAdvocate(int number) => By.Id($"defenceAdvocate{number}");
        public static By RemoveDisplayName(int number) => By.Id($"removeDisplayName{number}");
        public static By AddAnotherBtn = By.Id("addEndpoint");
        public static By NextButton = By.Id(("nextButton"));
    }
}
