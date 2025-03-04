namespace UI.AutomationTests.Mappers;

public static class HearingRoleCodeMapper
{
    public static string MapToHearingRoleCode(this GenericTestRole role)
    {
        return role switch
        {
            GenericTestRole.Applicant => HearingTestData.HearingRoleCodes.Applicant,
            GenericTestRole.Intermediary => HearingTestData.HearingRoleCodes.Intermediary,
            GenericTestRole.Representative => HearingTestData.HearingRoleCodes.Representative,
            GenericTestRole.Respondent => HearingTestData.HearingRoleCodes.Respondent,
            GenericTestRole.Interpreter => HearingTestData.HearingRoleCodes.Interpreter,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}