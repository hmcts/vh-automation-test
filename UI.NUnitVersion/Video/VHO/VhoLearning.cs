namespace UI.NUnitVersion.Video.VHO;

public class VhoLearning : VideoWebUiTest
{
    [Test]
    [Ignore("Using to build the command centre page object model")]
    public void METHOD()
    {
        var vhoVenueSelectionPage = LoginAsVho(HearingTestData.VhOfficerUsername, EnvConfigSettings.UserPassword);
        var venues = new List<string>()
        {
            "Birmingham Civil and Family Justice Centre",
            "Birmingham Social Security and Child Support Tribunal"
        };
        vhoVenueSelectionPage.SelectHearingsByVenues(venues);
    }
}