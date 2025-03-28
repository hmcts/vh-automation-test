SET XACT_ABORT ON;
GO;
BEGIN TRANSACTION;

-- auto_aw.judge_02@hearings.reform.hmcts.net
-- VH-GENERIC-ACCOUNT-0

MERGE INTO JudiciaryPerson AS target
USING (VALUES
    ('3DBB214B-9080-43B4-9EBA-A8877989031D',
     '3DBB214B-9080-43B4-9EBA-A8877989031D',
     'VH-GENERIC-ACCOUNT-0',
     null,
     'Judge',
     'Judge',
     'Automation Test Judge',
     'auto_aw.judge_02@hearings.reform.hmcts.net',
     '2024-01-05 12:21:39.0000000',
     '2024-01-05 12:22:15.0000000',
     0,
     0,
     null,
     null,
     1,
     0,
     null),
    ('63FE82B7-C9BD-4BE2-94F4-8DF4C0A18BAF',
     '63fe82b7-c9bd-4be2-94f4-8df4c0a18baf',
     'VH-GENERIC-ACCOUNT-02',
     'Mr',
     'Auto',
     'Panel',
     'Automation Test Panel Member',
     'auto_aw.panelmember_01@hearings.reform.hmcts.net',
     '2024-01-05 12:21:39.0000000',
     '2024-01-05 12:22:15.0000000',
     0,
     0,
     null,
     '1234567890',
     1,
     0,
     null)
) AS source (Id, ExternalRefId, PersonalCode, Title, KnownAs, Surname, Fullname, Email, CreatedDate, UpdatedDate, HasLeft, Leaver, LeftOn, WorkPhone, IsGeneric, Deleted, DeletedOn)
ON (target.PersonalCode = source.PersonalCode)

WHEN MATCHED THEN
    UPDATE SET
        target.ExternalRefId = source.ExternalRefId,
        target.Title = source.Title,
        target.KnownAs = source.KnownAs,
        target.Surname = source.Surname,
        target.Fullname = source.Fullname,
        target.Email = source.Email,
        target.UpdatedDate = source.UpdatedDate,
        target.HasLeft = source.HasLeft,
        target.Leaver = source.Leaver,
        target.LeftOn = source.LeftOn,
        target.WorkPhone = source.WorkPhone,
        target.IsGeneric = source.IsGeneric,
        target.Deleted = source.Deleted,
        target.DeletedOn = source.DeletedOn

WHEN NOT MATCHED THEN
    INSERT (Id, ExternalRefId, PersonalCode, Title, KnownAs, Surname, Fullname, Email, CreatedDate, UpdatedDate, HasLeft, Leaver, LeftOn, WorkPhone, IsGeneric, Deleted, DeletedOn)
    VALUES (source.Id, source.ExternalRefId, source.PersonalCode, source.Title, source.KnownAs, source.Surname, source.Fullname, source.Email, source.CreatedDate, source.UpdatedDate, source.HasLeft, source.Leaver, source.LeftOn, source.WorkPhone, source.IsGeneric, source.Deleted, source.DeletedOn);

SELECT *
FROM dbo.JudiciaryPerson
WHERE PersonalCode IN ('VH-GENERIC-ACCOUNT-0', 'VH-GENERIC-ACCOUNT-02');

COMMIT TRANSACTION;
SET XACT_ABORT OFF;