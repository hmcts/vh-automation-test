using System.ComponentModel;

namespace UI.PageModels.Dtos;

public enum GenericTestRole
{
    Appellant,
    Barrister,
    Expert,
    Intermediary,
    [Description("Litigant in person")] LitigantInPerson,
    [Description("Litigation friend")] LitigationFriend,
    [Description("MacKenzie friend")] MacKenzieFriend,
    Representative,
    Solicitor,
    Witness,
    Interpreter,
    Applicant,
    Respondent
    
    
}