using UI.PageModels.Pages.Video;

namespace UI.NUnitVersion.Video;

public abstract class VideoWebUiTest
{
    /// <summary>
    /// This property is used to book a hearing and publish the success to the test reporter
    /// </summary>
    protected IVhDriver BookingDriver;
    
    /// <summary>
    /// These drivers will store the participants' drivers for a given hearing
    /// </summary>
    protected Dictionary<string, IVhDriver> ParticipantDrivers = new();

    protected JudgeHearingListPage LoginAsJudge(string username, string password)
    {
        // create a driver
        // add to list
        // VhDrivers.Add(username, driver);
        // navigate to video web login page (i.e. /vh-sign-in to avoid idp selection screen) 
        // return driver
        throw new NotImplementedException();
    }
    
    protected VhoVenueSelectionPage LoginAsVho(string username, string password)
    {
        // create a driver
        // add to list
        // VhDrivers.Add(username, driver);
        // navigate to video web login page (i.e. /vh-sign-in to avoid idp selection screen) 
        // return driver
        throw new NotImplementedException();
    }
    
    protected StaffMemberHearingListPage LoginAsStaffMember(string username, string password)
    {
        // create a driver
        // add to list
        // VhDrivers.Add(username, driver);
        // navigate to video web login page (i.e. /vh-sign-in to avoid idp selection screen) 
        // return driver
        throw new NotImplementedException();
    }
    
    protected ParticipantHearingListPage LoginAsParticipant(string username, string password)
    {
        // create a driver
        // add to list
        // VhDrivers.Add(username, driver);
        // navigate to video web login page (i.e. /vh-sign-in to avoid idp selection screen) 
        // return driver
        throw new NotImplementedException();
    }

    protected void SignOutAs(string username)
    {
        throw new NotImplementedException();
    }
    
    protected void SignOutAllUsers()
    {
        // loop through all users in VhDrivers and sign out
        throw new NotImplementedException();
    }
}