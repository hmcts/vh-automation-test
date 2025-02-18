USE VhBookings;
GO;
SET XACT_ABORT ON
GO;
declare @organisationId BIGINT;
BEGIN TRANSACTION;

-- Upsert organisation 'AutoOrglnyI'
BEGIN
    IF EXISTS (SELECT * FROM dbo.Organisation WHERE Name = 'AutoOrglnyI')
        BEGIN
            PRINT ('Updating: AutoOrglnyI')
            Update dbo.Organisation
            SET Name       = 'AutoOrglnyI',
                UpdatedDate = CURRENT_TIMESTAMP
            WHERE Name = 'AutoOrglnyI'

        END
    ELSE
        BEGIN
            PRINT ('Adding: AutoOrglnyI')
            insert into dbo.Organisation (Name, CreatedDate, UpdatedDate)
            values ('AutoOrglnyI', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
        END
END

-- Upsert organisation 'Prosacco Inc and Sons'
BEGIN
    IF EXISTS (SELECT * FROM dbo.Organisation WHERE Name = 'Prosacco Inc and Sons')
        BEGIN
            PRINT ('Updating: ')
            Update dbo.Organisation
            SET Name       = 'Prosacco Inc and Sons',
                UpdatedDate = CURRENT_TIMESTAMP
            WHERE Name = ''
        END
    ELSE
        BEGIN
            PRINT ('Adding: ')
            insert into dbo.Organisation (Name, CreatedDate, UpdatedDate)
            values ('Prosacco Inc and Sons', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
        END
END

-- Upsert person 'auto_vw.individual_60@hmcts.net'
BEGIN
    IF EXISTS (SELECT * FROM dbo.Person WHERE ContactEmail = 'auto_vw.individual_60@hmcts.net')
        BEGIN
            PRINT ('Updating: auto_vw.individual_60@hmcts.net')
            Update dbo.Person
            SET Title           = 'Mr',
                FirstName       = 'Automation_Arnold',
                LastName        = 'Automation_Koelpin',
                MiddleNames     = '',
                Username        = 'auto_vw.individual_60@hearings.reform.hmcts.net',
                TelephoneNumber = '07021234567',
                OrganisationId  = NULL
            WHERE ContactEmail = 'auto_vw.individual_60@hmcts.net'
        END
    ELSE
        BEGIN
            PRINT ('Adding: auto_vw.individual_60@hmcts.net')
            insert into dbo.Person(Id, Title, FirstName, LastName, MiddleNames, Username, ContactEmail,
                                   TelephoneNumber, OrganisationId, CreatedDate, UpdatedDate)
            values ('A2F1E690-2DCB-40BE-AACA-EA3DDDD575FC', 'Mr', 'Automation_Arnold', 'Automation_Koelpin', '',
                    'auto_vw.individual_60@hearings.reform.hmcts.net', 'auto_vw.individual_60@hmcts.net',
                    '07021234567', NULL, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
        END
END

-- Upsert person 'auto_vw.representative_139@hmcts.net'
BEGIN
    IF EXISTS (SELECT * FROM dbo.Person WHERE ContactEmail = 'auto_vw.representative_139@hmcts.net')
        BEGIN
            PRINT ('Updating: auto_vw.representative_139@hmcts.net')
            SELECT @organisationId = id FROM dbo.Organisation WHERE Name LIKE 'AutoOrglnyI';
            Update dbo.Person
            SET Title           = 'Mr',
                FirstName       = 'Auto_VW',
                LastName        = 'Representative_139',
                MiddleNames     = '',
                Username        = 'auto_vw.representative_139@hearings.reform.hmcts.net',
                TelephoneNumber = '07021234567',
                OrganisationId  = @organisationId
            WHERE ContactEmail = 'auto_vw.representative_139@hmcts.net'
        END
    ELSE
        BEGIN
            PRINT ('Adding: auto_vw.representative_139@hmcts.net')
            SELECT @organisationId = id FROM dbo.Organisation WHERE Name LIKE 'AutoOrglnyI';
            insert into dbo.Person(Id, Title, FirstName, LastName, MiddleNames, Username, ContactEmail,
                                   TelephoneNumber, OrganisationId, CreatedDate, UpdatedDate)
            values ('DD634DB5-F933-4448-BD19-8E881047EE77', 'Mr', 'Auto_VW', 'Representative_139', '',
                    'auto_vw.representative_139@hearings.reform.hmcts.net', 'auto_vw.representative_139@hmcts.net',
                    '07021234567', @organisationId, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
        END
END

-- Upsert person 'auto_vw.individual_137@hmcts.net'
BEGIN
    IF EXISTS (SELECT * FROM dbo.Person WHERE ContactEmail = 'auto_vw.individual_137@hmcts.net')
        BEGIN
            PRINT ('Updating: auto_vw.individual_137@hmcts.net')
            Update dbo.Person
            SET Title           = 'Mr',
                FirstName       = 'Auto_VW',
                LastName        = 'Individual_137',
                MiddleNames     = '',
                Username        = 'auto_vw.individual_137@hearings.reform.hmcts.net',
                TelephoneNumber = '07021234567',
                OrganisationId  = NULL
            WHERE ContactEmail = 'auto_vw.individual_137@hmcts.net'
        END
    ELSE
        BEGIN
            PRINT ('Adding: auto_vw.individual_137@hmcts.net')
            insert into dbo.Person(Id, Title, FirstName, LastName, MiddleNames, Username, ContactEmail,
                                   TelephoneNumber, OrganisationId, CreatedDate, UpdatedDate)
            values ('04CD8089-E8E8-47E4-9FE1-CF6AFA8D02FE', 'Mr', 'Auto_VW', 'Individual_137', '',
                    'auto_vw.individual_137@hearings.reform.hmcts.net', 'auto_vw.individual_137@hmcts.net',
                    '07021234567', null, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
        END
END

-- Upsert person 'auto_vw.representative_157@hmcts.net'
BEGIN
    IF EXISTS (SELECT * FROM dbo.Person WHERE ContactEmail = 'auto_vw.representative_157@hmcts.net')
        BEGIN
            PRINT ('Updating: auto_vw.representative_157@hmcts.net')
            SELECT @organisationId = id FROM dbo.Organisation WHERE Name LIKE 'Prosacco Inc and Sons';
            Update dbo.Person
            SET Title           = 'Mr',
                FirstName       = 'Automation_Torrance',
                LastName        = 'Automation_Moen',
                MiddleNames     = '',
                Username        = 'auto_vw.representative_157@hearings.reform.hmcts.net',
                TelephoneNumber = '07021234567',
                OrganisationId  = @organisationId
            WHERE ContactEmail = 'auto_vw.representative_157@hmcts.net'
        END
    ELSE
        BEGIN
            PRINT ('Adding: auto_vw.representative_157@hmcts.net')
            SELECT @organisationId = id FROM dbo.Organisation WHERE Name LIKE 'Prosacco Inc and Sons';
            insert into dbo.Person(Id, Title, FirstName, LastName, MiddleNames, Username, ContactEmail,
                                   TelephoneNumber, OrganisationId, CreatedDate, UpdatedDate)
            values ('6C610F6F-F3D8-433A-9846-E8081D09F6D3', 'Mr', 'Automation_Torrance', 'Automation_Moen', '',
                    'auto_vw.representative_157@hearings.reform.hmcts.net', 'auto_vw.representative_157@hmcts.net',
                    '07021234567', @organisationId, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
        END
END


BEGIN
    IF EXISTS (SELECT * FROM dbo.Person WHERE ContactEmail = 'automation_claimant_interpreter_1@hmcts.net')
        BEGIN
            PRINT ('Updating: automation_claimant_interpreter_1@hmcts.net')
            Update dbo.Person
            SET Title           = 'Mrs',
                FirstName       = 'Automation_Claimant',
                LastName        = 'Interpreter_1',
                MiddleNames     = 'MiddleName1',
                Username        = 'automation_claimant_interpreter_1@hearings.reform.hmcts.net',
                TelephoneNumber = '01234567890'
            WHERE ContactEmail = 'automation_claimant_interpreter_1@hmcts.net'
        END
    ELSE
        BEGIN
            PRINT ('Adding: automation_claimant_interpreter_1@hmcts.net')
            insert into dbo.Person(Id, Title, FirstName, LastName, MiddleNames, Username, ContactEmail,
                                   TelephoneNumber, OrganisationId, CreatedDate, UpdatedDate)
            values ('663EEE0C-0884-4B80-AD9E-234BC179A4EF', 'Mrs', 'Automation_Claimant', 'Interpreter_1', 'MiddleName1',
                    'automation_claimant_interpreter_1@hearings.reform.hmcts.net', 'automation_claimant_interpreter_1@hmcts.net',
                    '01234567890', null, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
        END
END

SELECT *
FROM dbo.Person
WHERE ContactEmail in
      ('auto_vw.individual_60@hmcts.net', 'auto_vw.representative_139@hmcts.net', 'auto_vw.individual_137@hmcts.net',
       'auto_vw.representative_157@hmcts.net', 'automation_claimant_interpreter_1@hmcts.net')
COMMIT TRANSACTION;
SET XACT_ABORT OFF