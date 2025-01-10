using System.Text.RegularExpressions;
using Notify.Interfaces;

namespace UI.AutomationTests.EmailNotifications;

public class EmailNotificationService
{
    private IAsyncNotificationClient NotifyApiClient { get; } = VhApiClientFactory.CreateNotificationApiClient();
    private readonly Regex _tempPassword = new(@"(?<=temporary password: )[\S]+");
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
            "b3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"
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
            x.template.id == _emails[EmailTemplates.SecondEmailNewUserConfirmation]);
        return _tempPassword.Match(newUserEmails!.body).Value;
    }
    
    public async Task ValidateEmailReceived(string contactEmail, EmailTemplates emailTemplate)
    {
        var allNotifications = await NotifyApiClient.GetNotificationsAsync("email");
        var notifyContactEmails = allNotifications.notifications.Where(x =>
                x.emailAddress == contactEmail &&
                x.template.id == _emails[emailTemplate])
            .ToList();
        var emailExists = notifyContactEmails.Exists(e => DateTime.Parse(e.createdAt) > DateTime.UtcNow.AddMinutes(-1));
        Assert.That(emailExists, $"Email with template {emailTemplate} was not received by {contactEmail} in the last 60 seconds");
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