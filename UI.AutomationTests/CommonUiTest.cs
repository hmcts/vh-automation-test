using System.Net;
using BookingsApi.Client;
using BookingsApi.Contract.V1.Requests.Enums;
using UserApi.Client;

namespace UI.AutomationTests;

public abstract class CommonUiTest
{
    protected List<string> TestHearingIds = new();
    protected List<string> CreatedUsers = new();
    protected BookingsApiClient BookingsApiClient;
    protected UserApiClient UserApiClient;
    protected async Task<JusticeUserResponse> CreateVhTeamLeaderJusticeUserIfNotExist(string username)
    {
        var matchedUsers = await BookingsApiClient.GetJusticeUserListAsync(username, true);
        var justiceUser = matchedUsers.FirstOrDefault(x =>
            x.ContactEmail.Equals(username, StringComparison.InvariantCultureIgnoreCase));
        if (justiceUser == null)
        {
            justiceUser = await BookingsApiClient.AddJusticeUserAsync(new AddJusticeUserRequest
            {
                Username = username,
                ContactEmail = username,
                ContactTelephone = null,
                FirstName = "Auto",
                LastName = "VHoteamleader",
                Roles = new List<JusticeUserRole> { JusticeUserRole.VhTeamLead },
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
                Roles = new List<JusticeUserRole> {JusticeUserRole.VhTeamLead}
            });
        }

        TestContext.WriteLine($"Using justice user for test {justiceUser.ContactEmail}");

        return justiceUser;
    }
    
    [OneTimeTearDown]
    protected async Task OneTimeTearDown()
    {
        await DeleteHearings();
        await DeleteUsers();
    }

    private async Task DeleteUsers()
    {
        foreach (var userPrincipleName in CreatedUsers)
            try
            {
                await UserApiClient.DeleteUserAsync(userPrincipleName);
            }
            catch(UserApiException e)
            {
                TestContext.WriteLine(e.StatusCode == (int)HttpStatusCode.NotFound
                    ? $"User {userPrincipleName} not found"
                    : $"Failed to remove user {userPrincipleName} - {e.Message}");
            }
    }

    private async Task DeleteHearings()
    {
        List<string> removedHearings = new();
        foreach (var hearingId in TestHearingIds)
        {
            if (Guid.TryParse(hearingId, out var guid))
            {
                try
                {
                    TestContext.WriteLine($"Removing Hearing {guid}");
                    await BookingsApiClient.RemoveHearingAsync(guid);
                    removedHearings.Add(hearingId);
                }
                catch (BookingsApiException e)
                {
                    TestContext.WriteLine(e.StatusCode == (int)HttpStatusCode.NotFound
                        ? $"Hearing {guid} not found"
                        : $"Failed to remove hearing {guid} - {e.Message}");
                }
                catch (Exception e)
                {
                    TestContext.WriteLine($"Failed to remove hearing {guid} - {e.Message}");
                }
            }
        }
        // remove the hearing from the list so that it is not attempted to be removed again
        TestHearingIds.RemoveAll(id => removedHearings.Contains(id));
    }
}