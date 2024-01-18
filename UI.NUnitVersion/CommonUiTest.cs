using BookingsApi.Client;
using BookingsApi.Contract.V1.Requests.Enums;

namespace UI.NUnitVersion;

public abstract class CommonUiTest
{
    protected List<string> TestHearingIds = new();
    protected BookingsApiClient BookingsApiClient;
    protected async Task<JusticeUserResponse> CreateVhTeamLeaderJusticeUserIfNotExist(string username)
    {
        var matchedUsers = await BookingsApiClient.GetJusticeUserListAsync(username, true);
        var justiceUser = matchedUsers.FirstOrDefault(x =>
            x.ContactEmail.Equals(username, StringComparison.InvariantCultureIgnoreCase));
        if (justiceUser == null)
        {
            justiceUser = await BookingsApiClient.AddJusticeUserAsync(new AddJusticeUserRequest()
            {
                Username = username,
                ContactEmail = username,
                ContactTelephone = null,
                FirstName = "Auto",
                LastName = "VHoteamleader",
                Roles = new List<JusticeUserRole>()
                {
                    JusticeUserRole.VhTeamLead
                },
                CreatedBy = "automation test framework"
            });
            TestContext.WriteLine($"Created user {justiceUser.ContactEmail}");
        }

        if (justiceUser.Deleted)
        {
            TestContext.WriteLine("Restoring deleted user {justiceUser.ContactEmail}");
            await BookingsApiClient.RestoreJusticeUserAsync(new RestoreJusticeUserRequest()
            {
                Id = justiceUser.Id, Username = justiceUser.Username
            });
        }

        if (!justiceUser.IsVhTeamLeader)
        {
            TestContext.WriteLine("Updated justice user to be a Team Leader");
            await BookingsApiClient.EditJusticeUserAsync(new EditJusticeUserRequest()
            {
                Id = justiceUser.Id, Username = justiceUser.Username,
                Roles = new List<JusticeUserRole>() {JusticeUserRole.VhTeamLead}
            });
        }

        TestContext.WriteLine($"Using justice user for test {justiceUser.ContactEmail}");

        return justiceUser;
    }
    
    [OneTimeTearDown]
    protected async Task OneTimeTearDown()
    {
        foreach (var hearingId in TestHearingIds)
        {
            if (Guid.TryParse(hearingId, out var guid))
            {
                TestContext.WriteLine($"Removing Hearing {guid}");
                await BookingsApiClient.RemoveHearingAsync(guid);
            }
        }
    }
}