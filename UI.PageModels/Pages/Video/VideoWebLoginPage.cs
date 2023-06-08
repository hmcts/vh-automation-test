using OpenQA.Selenium;
using UI.PageModels.Pages.Video.Participant;
using UI.PageModels.Pages.Video.Vho;

namespace UI.PageModels.Pages.Video;

public class VideoWebLoginPage : VhLoginPage
{
    public VideoWebLoginPage(IWebDriver driver, int defaultWaitTime) : base(driver, defaultWaitTime)
    {
    }

    public JudgeHearingListPage LogInAsJudge(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new JudgeHearingListPage(Driver, DefaultWaitTime);
    }

    public ParticipantHearingListPage LogInAsParticipant(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new ParticipantHearingListPage(Driver, DefaultWaitTime);
    }

    public StaffMemberHearingListPage LogInAsStaffMember(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new StaffMemberHearingListPage(Driver, DefaultWaitTime);
    }

    public VhoVenueSelectionPage LogInAsVho(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new VhoVenueSelectionPage(Driver, DefaultWaitTime);
    }
}