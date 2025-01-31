using System.Text.RegularExpressions;
using Notify.Interfaces;
using Notify.Models;
using Polly;

namespace UI.AutomationTests.EmailNotifications;

public class EmailNotificationService
{
    private IAsyncNotificationClient NotifyApiClient { get; } = VhApiClientFactory.CreateNotificationApiClient();
    private readonly Regex _tempPassword = new(@"(?<=temporary password: )[\S]+");
    private NotificationList _notificationList;
    private readonly Dictionary<EmailTemplates, string> _emails = new()
    {
        {
            EmailTemplates.FirstEmailAllNewUsers, 
            "6c9be8bd-9aaa-468c-ad73-340fb0919b21"
        },
        {
            EmailTemplates.SecondEmailNewUserConfirmation,
            "625bb8c7-b70b-4fde-867e-9e365285d756"
        },
        {
            EmailTemplates.ExistingParticipantConfirmation,
            "7458e52d-3954-4f33-bd2d-0a7d2de295fc"
        },
        {
            EmailTemplates.ExistingParticipantConfirmationMultiDay,
            "3d83ee22-71ef-47f6-9557-bdaf0c0eecae"
        },
        {
            EmailTemplates.ExistingProfessionalConfirmation,
            "bd64e5d2-610f-449a-a925-3db10f913019"
        },
        {
            EmailTemplates.ExistingProfessionalConfirmationMultiDay,
            "bc278f80-69a5-4e91-a0ef-4bfe2a52796e"
        },
        { 
            EmailTemplates.EmailReminder48Hour, 
            "cc5cbdca-6614-484d-8b2d-5446ebccb47b" 
        },
        { 
            EmailTemplates.JudgeHearingConfirmation, 
            "811125fe-4cab-4829-88ed-d3e7d4689cdd" 
        },
        {
            EmailTemplates.JudgeHearingConfirmationMultiDay,
            "04cd937d-c6eb-4932-a040-469123afef67"
        },
        {
            EmailTemplates.HearingAmendment,
            "197d2b04-a600-41ae-bf68-8021d6ea0057"
        },
        {
            EmailTemplates.HearingAmendmentProfessional,
            "bc278f80-69a5-4e91-a0ef-4bfe2a52796e"
        },
        {
            EmailTemplates.HearingAmendmentJudge,
            "3210895a-c096-4029-b43e-9fde4642a254"
        }
    };

    public async Task<string> GetTempPasswordForUser(string contactEmail, string caseNumber)
    {
        const int maxRetryAttempts = 3;
        const int retryDelaySeconds = 5;
        
        var retryPolicy = Policy
            .HandleResult<string>(string.IsNullOrEmpty)
            .WaitAndRetryAsync(maxRetryAttempts, _ => TimeSpan.FromSeconds(retryDelaySeconds),
                (_, _, retryCount, context) =>
                {
                    // Log or handle the retry attempt here
                    context["RetryCount"] = retryCount;
                });

        var tempPassword = await retryPolicy.ExecuteAsync(async (context) =>
        {
            _ = context.TryGetValue("RetryCount", out var retryCount) ? (int)retryCount : 0;
            Console.WriteLine($"Executing retry attempt {retryCount} for {contactEmail}");

            var allNotifications = await NotifyApiClient.GetNotificationsAsync("email");
            var newUserEmail = allNotifications.notifications.Find(x =>
                x.emailAddress == contactEmail && x.body.Contains(caseNumber) &&
                x.template.id == _emails[EmailTemplates.SecondEmailNewUserConfirmation]);
            if(newUserEmail == null)
            {
                await TestContext.Out.WriteLineAsync("New user email not found");
                return null;
            }
            var match = _tempPassword.Match(newUserEmail.body);
            return match.Success ? match.Value : null;
        }, new Context());

        if (string.IsNullOrEmpty(tempPassword))
        {
            throw new Exception($"Temporary password email not found for {contactEmail}");
        }

        return tempPassword;
    }

    public async Task PullNotificationList()
    {
        _notificationList = await NotifyApiClient.GetNotificationsAsync("email");
    }

    public async Task ValidateEmailReceived(string contactEmail, EmailTemplates emailTemplate, string caseName, string caseNumber)
    {
        var emailExists = await QueryNotifyForEmail(contactEmail, emailTemplate, caseName, caseNumber);
        Assert.That(emailExists, $"Email with template {emailTemplate} was not sent to {contactEmail} with case name {caseName}");
    }
    
    private async Task<bool> QueryNotifyForEmail(string contactEmail, EmailTemplates emailTemplate, string caseName, string caseNumber)
    {
        const int maxRetryAttempts = 3;
        const int retryDelaySeconds = 5;
        
        var retryPolicy = Policy
            .HandleResult<bool>(x => !x)
            .WaitAndRetryAsync(maxRetryAttempts, _ => TimeSpan.FromSeconds(retryDelaySeconds), async (_, _, retryCount, context) =>
                {
                    // Log or handle the retry attempt here
                    context["RetryCount"] = retryCount;
                    await PullNotificationList();
                });

        return await retryPolicy.ExecuteAsync(async (context) =>
        {
            _ = context.TryGetValue("RetryCount", out var retryCount) ? (int)retryCount : 0;
            Console.WriteLine($"Executing retry attempt {retryCount} for {contactEmail}");

            var emailExists = _notificationList.notifications.Exists(x => x.emailAddress == contactEmail &&
                                                                          x.template.id == _emails[emailTemplate] &&
                                                                          (x.body.Contains(caseName) ||
                                                                           x.subject.Contains(caseName) ||
                                                                           x.body.Contains(caseNumber) ||
                                                                           x.subject.Contains(caseNumber)
                                                                          ));
            if (!emailExists)
            {
                await TestContext.Out.WriteLineAsync($"Email not found {contactEmail}, {emailTemplate}, {caseName}");
            }
            return emailExists;
        }, new Context());
    }
}

public enum EmailTemplates
{
    FirstEmailAllNewUsers,
    SecondEmailNewUserConfirmation,
    ExistingParticipantConfirmation,
    ExistingParticipantConfirmationMultiDay,
    ExistingProfessionalConfirmation,
    ExistingProfessionalConfirmationMultiDay,
    JudgeHearingConfirmation,
    JudgeHearingConfirmationMultiDay,
    HearingAmendment,
    HearingAmendmentProfessional,
    HearingAmendmentJudge,
    EmailReminder48Hour
}