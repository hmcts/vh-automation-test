@web

Feature:Check alerts are sent correctly to command centre
	
Scenario: Alert Failed self test - No to Camera

	Given I log in as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net"
	And I select book a hearing
	And I want to create a hearing with case details 
	| Case Number | Case Name              | Case Type | Hearing Type        |
	| AA          | AutomationTestCaseName | Civil     | Enforcement Hearing |
	And the hearing has the following schedule details
	| Schedule Date | Duration Hour | Duration Minute |
	|               | 0             | 30              |
	And I want to Assign a Judge with courtroom details
	| Judge or Courtroom Account                 |
	| auto_aw.judge_03@hearings.reform.hmcts.net |   
	And I want to create a Hearing for
	| Party    | Role        | Id                                               |
	| Claimant | Witness     | auto_vw.individual_05@hearings.reform.hmcts.net  |
	And With video Access points details
	| Display Name | Advocate |
	|              |          |		
	And I set any other information
	| Record Hearing | Other information   | 
	|                | This is a test info |
	And I book the hearing
    Then A hearing should be created
	And I log off 
	Then all participants log in to video web
	And participant has joined and progressed to waiting room without completing self test
	Given I login to VHO in video url as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net" for existing hearing
	And I choose from hearing lists
		| Select your hearing lists                  |
		| Birmingham Civil and Family Justice Centre |
	When the Video Hearings Officer check alerts for this hearing
	Then the the Video Hearings Officer see the alert Failed self-test (No to Camera) participant F & L name
	And I log off 
	And everyone signs out



	Scenario: Alert Failed self test - No to Microphone
	Given I log in as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net"
	And I select book a hearing
	And I want to create a hearing with case details 
	| Case Number | Case Name              | Case Type | Hearing Type        |
	| AA          | AutomationTestCaseName | Civil     | Enforcement Hearing |
	And the hearing has the following schedule details
	| Schedule Date | Duration Hour | Duration Minute |
	|               | 0             | 30              |
	And I want to Assign a Judge with courtroom details
	| Judge or Courtroom Account                 |
	| auto_aw.judge_03@hearings.reform.hmcts.net |   
	And I want to create a Hearing for
	| Party    | Role        | Id                                               |
	| Claimant | Witness     | auto_vw.individual_05@hearings.reform.hmcts.net  |
	And With video Access points details
	| Display Name | Advocate |
	|              |          |		
	And I set any other information
	| Record Hearing | Other information   | 
	|                | This is a test info |
	And I book the hearing
    Then A hearing should be created
	And I log off 
	Then all participants log in to video web
	And participant has joined and progressed to waiting room without completing self test microphone
	Given I login to VHO in video url as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net" for existing hearing
	And I choose from hearing lists
		| Select your hearing lists                  |
		| Birmingham Civil and Family Justice Centre |
	When the Video Hearings Officer check alerts for this hearing
	Then the the Video Hearings Officer see the alert Failed self-test (No to Microphone) participant F & L name


	Scenario: Alert Failed self test - No to Video
	Given I log in as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net"
	And I select book a hearing
	And I want to create a hearing with case details 
	| Case Number | Case Name              | Case Type | Hearing Type        |
	| AA          | AutomationTestCaseName | Civil     | Enforcement Hearing |
	And the hearing has the following schedule details
	| Schedule Date | Duration Hour | Duration Minute |
	|               | 0             | 30              |
	And I want to Assign a Judge with courtroom details
	| Judge or Courtroom Account                 |
	| auto_aw.judge_03@hearings.reform.hmcts.net |   
	And I want to create a Hearing for
	| Party    | Role        | Id                                               |
	| Claimant | Witness     | auto_vw.individual_05@hearings.reform.hmcts.net  |
	And With video Access points details
	| Display Name | Advocate |
	|              |          |		
	And I set any other information
	| Record Hearing | Other information   | 
	|                | This is a test info |
	And I book the hearing
    Then A hearing should be created
	And I log off 
	Then all participants log in to video web
	And participant has joined and progressed to waiting room without completing self test video
	Given I login to VHO in video url as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net" for existing hearing
	And I choose from hearing lists
		| Select your hearing lists                  |
		| Birmingham Civil and Family Justice Centre |
	When the Video Hearings Officer check alerts for this hearing
	Then the the Video Hearings Officer see the alert Failed self-test (No to Video) participant F & L name


	Scenario: Alert Failed self test - Incomplete
	Given I log in as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net"
	And I select book a hearing
	And I want to create a hearing with case details 
	| Case Number | Case Name              | Case Type | Hearing Type        |
	| AA          | AutomationTestCaseName | Civil     | Enforcement Hearing |
	And the hearing has the following schedule details
	| Schedule Date | Duration Hour | Duration Minute |
	|               | 0             | 30              |
	And I want to Assign a Judge with courtroom details
	| Judge or Courtroom Account                 |
	| auto_aw.judge_03@hearings.reform.hmcts.net |   
	And I want to create a Hearing for
	| Party    | Role        | Id                                               |
	| Claimant | Witness     | auto_vw.individual_05@hearings.reform.hmcts.net  |
	And With video Access points details
	| Display Name | Advocate |
	|              |          |		
	And I set any other information
	| Record Hearing | Other information   | 
	|                | This is a test info |
	And I book the hearing
    Then A hearing should be created
	And I log off 
	Then all participants log in to video web
	And participant has joined and progressed to waiting room without completing self test Incomplete
	Given I login to VHO in video url as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net" for existing hearing
	And I choose from hearing lists
		| Select your hearing lists                  |
		| Birmingham Civil and Family Justice Centre |
	When the Video Hearings Officer check alerts for this hearing
	Then the the Video Hearings Officer see the alert Failed self-test (incomplete) participant F & L name

	Scenario: Alert Failed self test - Disconnected
	Given I log in as "auto_aw.videohearingsofficer_04@hearings.reform.hmcts.net"
	And I select book a hearing
	And I want to create a hearing with case details 
	| Case Number | Case Name              | Case Type | Hearing Type        |
	| AA          | AutomationTestCaseName | Civil     | Enforcement Hearing |
	And the hearing has the following schedule details
	| Schedule Date | Duration Hour | Duration Minute |
	|               | 0             | 30              |
	And I want to Assign a Judge with courtroom details
	| Judge or Courtroom Account                 |
	| auto_aw.judge_03@hearings.reform.hmcts.net |   
	And I want to create a Hearing for
	| Party    | Role        | Id                                               |
	| Claimant | Witness     | auto_vw.individual_06@hearings.reform.hmcts.net  |
	And With video Access points details
	| Display Name | Advocate |
	|              |          |		
	And I set any other information
	| Record Hearing | Other information   | 
	|                | This is a test info |
	And I book the hearing
    Then A hearing should be created
	And I log off 
	Then all participants log in to video web
	And participant has joined and progressed to waiting room without completing self test disconnected
	Given I login to VHO in video url as "auto_aw.videohearingsofficer_03@hearings.reform.hmcts.net" for existing hearing
	And I choose from hearing lists
		| Select your hearing lists                  |
		| Birmingham Civil and Family Justice Centre |
	When the Video Hearings Officer check alerts for this hearing
	Then the the Video Hearings Officer see the alert Failed self-test (disconnected) participant F & L name
	And I log off
	And everyone signs out