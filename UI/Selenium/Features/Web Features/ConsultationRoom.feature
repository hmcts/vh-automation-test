@ConsultationRoom
@web

Feature: Consultation room 
    Participants can join the consultation room

Scenario: Judge invites the participant to join the consultation room
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
    | auto_aw.judge_02@hearings.reform.hmcts.net |   
    And I want to create a Hearing for
    | Party     | Role               | Id                                  |
    | Claimant  | Litigant in person | auto_vw.individual_05@hearings.reform.hmcts.net    |
    And With video Access points details
    | Display Name | Advocate |
    |              |          | 
    And I set any other information
    | Record Hearing | Other information   | 
    |                | This is a test info |
    And I book the hearing
    Then A hearing should be created
    And I log off
    And all participants log in to video web
    And all participants have joined the hearing waiting room
    When the judge selects Enter consultation room
    And judge invites participant into the consultation
    And participant accepts the consultation room invitation
    Then judge checks participant joined the consultation room
    And all participants leave consultation room
    And everyone signs out

Scenario: Closed hearings - can join consultation room after hearing is closed
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
    | auto_aw.judge_02@hearings.reform.hmcts.net |   
    And I want to create a Hearing for
    | Party        | Role         | Id                                              |
    | Panel Member | Panel Member | auto_aw.panelmember_01@hearings.reform.hmcts.net |
    And With video Access points details
    | Display Name | Advocate |
    |              |          | 
    And I set any other information
    | Record Hearing | Other information   | 
    |                | This is a test info |
    And I book the hearing
    Then A hearing should be created
    And I log off
    And all participants log in to video web
    And all participants have joined the hearing waiting room
    And the judge starts the hearing
    And the judge checks that all participants have joined the hearing room
    And the judge closes the hearing
    When the judge selects Enter consultation room
    And the panel member selects Enter consultation room
    Then judge checks participant joined the consultation room
    And all participants leave consultation room
    And everyone signs out

Scenario: Participants start and leave Private Consultatin room
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
    | auto_aw.judge_02@hearings.reform.hmcts.net |   
    And I want to create a Hearing for
    | Party    | Role               | Id                                                  |
    | Claimant | Litigant in person | auto_vw.individual_06@hearings.reform.hmcts.net     |
    | Claimant | Representative     | auto_vw.representative_06@hearings.reform.hmcts.net |
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
    And all participants have joined the hearing waiting room
    When 'Litigant in person' start a private meeting and selects 'Representative'
    Then 'Representative' is in the private consultation room
    And 'Litigant in person' click on the leave button to leave the consultation room 
    And 'Representative' click on the leave button to leave the consultation room 
    And everyone signs out	

Scenario: JOH can start and leave Consultation room
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
    | auto_aw.judge_02@hearings.reform.hmcts.net |   
    And I want to create a Hearing for
    | Party        | Role         | Id                                              |
    | Panel Member | Panel Member | auto_aw.panelmember_01@hearings.reform.hmcts.net |
    And With video Access points details
    | Display Name | Advocate |
    |              |          | 
    And I set any other information
    | Record Hearing | Other information   | 
    |                | This is a test info |
    And I book the hearing
    Then A hearing should be created
    And I log off
    And all participants log in to video web
    And all participants have joined the hearing waiting room
    When the judge selects Enter consultation room
    And the panel member selects Enter consultation room
    Then judge checks participant joined the consultation room
    And all participants leave consultation room
    And everyone signs out
	And I log off
	Then all participants log in to video web
	And all participants have joined the hearing waiting room
	When 'Litigant in person' start a private meeting and selects 'Representative'
	Then 'Representative' is in the private consultation room
	And 'Litigant in person' click on the leave button to leave the consultation room 
	And 'Representative' click on the leave button to leave the consultation room 
	And everyone signs out	

Scenario: Consultation room: VHO can start and close consultaion
Given I log in as "auto_aw.videohearingsofficer_01@hearings.reform.hmcts.net"
	And I select book a hearing
	And I want to create a hearing with case details 
	| Case Number | Case Name              | Case Type | Hearing Type        |
	| AA          | AutomationTestCaseName | Civil     | Enforcement Hearing |
	And the hearing has the following schedule details
	| Schedule Date | Duration Hour | Duration Minute |
	|               | 0             | 30              |
	And I want to Assign a Judge with courtroom details
	| Judge or Courtroom Account                 |
	| auto_aw.judge_02@hearings.reform.hmcts.net |   
	And I want to create a Hearing for
	| Party     | Role               | Id                                  |
	| Claimant  | Litigant in person | auto_vw.individual_05@hearings.reform.hmcts.net    |

	And With video Access points details
	| Display Name | Advocate |
	|              |          | 
	And I set any other information
	| Record Hearing | Other information   | 
	|                | This is a test info |

	And I book the hearing
    Then A hearing should be created
	And I log off
	And all participants log in to video web
	And all participants have joined the hearing waiting room
	When Video Hearing Officer logs into video web as "auto_aw.videohearingsofficer_01@hearings.reform.hmcts.net"
	And selects hearing venue in the venue list
	And selects current hearing
	And starts a consultation with a judge
	And the invite into consultation room gets accepted
	Then check judge is in the consultation room
	And closes the consultation
	Then check the judge returns to the waiting room