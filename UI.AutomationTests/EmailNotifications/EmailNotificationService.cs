using System.Text.RegularExpressions;
using Notify.Interfaces;
using Notify.Models;

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

    public async Task<string> GetTempPasswordForUser(string contactEmail)
    {
        var allNotifications = await NotifyApiClient.GetNotificationsAsync("email");
        var newUserEmails = allNotifications.notifications.Find(x =>
            x.emailAddress == contactEmail && 
            x.template.id == _emails[EmailTemplates.SecondEmailNewUserConfirmation] &&
            DateTime.Parse(x.createdAt, DateTimeFormatInfo.CurrentInfo) > DateTime.UtcNow.AddMinutes(-2));
        return _tempPassword.Match(newUserEmails!.body).Value;
    }

    public async Task PullNotificationList()
    {
        _notificationList = await NotifyApiClient.GetNotificationsAsync("email");
    }

    public async Task ValidateEmailReceived(string contactEmail, EmailTemplates emailTemplate)
    {
        var emailExists = await QueryNotifyForEmail(contactEmail, emailTemplate);
        Assert.That(emailExists, $"Email with template {emailTemplate} was not sent to {contactEmail} in the last 5 minutes");
    }
    
    private async Task<bool> QueryNotifyForEmail(string contactEmail, EmailTemplates emailTemplate, bool retry = true)
    {
        var emailExists = _notificationList.notifications.Exists(x => x.emailAddress == contactEmail && 
                                                                      x.template.id == _emails[emailTemplate] && 
                                                                      DateTime.Parse(x.sentAt, DateTimeFormatInfo.CurrentInfo) > DateTime.UtcNow.AddMinutes(-5));
        if (!emailExists && retry)
        {
            //Sleep 10 seconds and try again as the email may not have been sent yet
            Thread.Sleep(10_000);
            await PullNotificationList();
            return await QueryNotifyForEmail(contactEmail, emailTemplate, false);
        }
        return emailExists;
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