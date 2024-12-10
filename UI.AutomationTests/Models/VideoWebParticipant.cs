using UI.AutomationTests.Drivers;

namespace UI.AutomationTests.Models;

public class VideoWebParticipant
{
    public string Username { get; set; }
    public JourneyType JourneyType { get; set; }
    public VhVideoWebPage VhVideoWebPage { get; set; }
    public IVhDriver Driver { get; set; }
}

public enum JourneyType
{
    Citizen,
    Representative,
    Judge,
    Vho,
    QuickLinkParticipant,
    StaffMember,
    Jvs
}