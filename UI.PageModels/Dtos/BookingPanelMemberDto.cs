namespace UI.PageModels.Dtos;

public class BookingPanelMemberDto
{
   
        public BookingPanelMemberDto (string personalCode, string username, string displayName, string phone)
        {
            PersonalCode = personalCode;
            Username = username;
            DisplayName = displayName;
            Phone = phone;
        }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public InterpreterLanguageDto? InterpreterLanguage { get; set; }
        public string PersonalCode { get; set; }
}

