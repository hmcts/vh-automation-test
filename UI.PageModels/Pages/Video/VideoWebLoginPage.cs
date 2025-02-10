using UI.PageModels.Pages.Video.Participant;
using UI.PageModels.Pages.Video.Vho;

namespace UI.PageModels.Pages.Video;

public class VideoWebLoginPage(IWebDriver driver, int defaultWaitTime) : VhLoginPage(driver, defaultWaitTime)
{
    public JudgeHearingListPage LogInAsJudge(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new JudgeHearingListPage(Driver, DefaultWaitTime);
    }

    public PanelMemberHearingListPage LoginAsPanelMember(string username, string password)
    {
        EnterLoginDetails(username,password);
        return new PanelMemberHearingListPage(Driver, DefaultWaitTime);
    }

    public ParticipantHearingListPage LogInAsParticipant(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new ParticipantHearingListPage(Driver, DefaultWaitTime);
    }

    public ParticipantHearingListPage LogInAsNewParticipant(string username, string password)
    {
        EnterLoginDetails(username, password);
        UpdateUserPassword(password);
        return new ParticipantHearingListPage(Driver, DefaultWaitTime);
    }
    public StaffMemberVenueListPage LogInAsStaffMember(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new StaffMemberVenueListPage(Driver, DefaultWaitTime);
    }

    public VhoVenueSelectionPage LogInAsVho(string username, string password)
    {
        EnterLoginDetails(username, password);
        return new VhoVenueSelectionPage(Driver, DefaultWaitTime);
    }
    
}