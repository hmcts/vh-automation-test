using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class Hearing
    {
        public Case Case { get; set; } = new Case();
        public BookingList BookingList { get; set; } = new BookingList();
        public List<string> Interpreters { get; set; } = new List<string>();
        public List<Participant> Participant { get; set; } = new List<Participant>();
        public string VHO { get; set; }
        public string JOH { get; set; }
        public HearingSchedule HearingSchedule { get; set; } = new HearingSchedule();
        public  Judge Judge { get; set; } = new Judge();
        public List<VideoAccessPoints> VideoAccessPoints { get; set; } = new List<VideoAccessPoints>();
        public OtherInformation OtherInformation { get; set; } = new OtherInformation();

        public string HearingId { get; set; }
    }

    public class Case
    {
        public string CaseNumber { get; set; }
        public string CaseName { get; set; }
        public string CaseType { get; set; }
        public string HearingType { get; set; }
    }

    public class HearingSchedule
    {
        public bool IsMultiHeariang { get; set; }
        public List<DateTime> HearingDate { get; set; }
        public DateTime HearingTime { get; set; }
        public string DurationHours { get; set; }
        public string DurationMinutes { get; set; }
        public string HearingVenue { get; set; }
        public string HearingRoom { get; set; }

    }

    public class Judge
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
    }

    public class Party
    {
        public string Name;
    }

    public class Role
    {
        public string Name;
    }

    public class Id
    {
        public string Name;
    }

    public class Participant
    {
        public Party Party { get; set; } = new Party();
        public Role Role { get; set; } = new Role();
        public string Id;
        public Name Name { get; set; } = new Name();
    }

    public class VideoAccessPoints
    {
        public string DisplayName { get; set; }
        public string Advocate { get; set; }
    }

    public class OtherInformation
    {
        public bool IsHearingRecorded { get; set; } = true;
        public string AnyOtherInfo { get; set; }
    }

    public class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class HearingList
    {
        public string HearingListID { get; set; }
        public string HearingListURL { get; set; }
        public string HearingListPhone { get; set; }

    }

    public class BookingList
    {
        public string TelephoneParticipantLink { get; set; }
        public string VideoParticipantLink { get; set; }

    }
}
