@web
@DeviceTest

Feature: Judge Invites Participants
	Judge invites the participants to meeting
	
@ignore
Scenario: Judge invites the participants to join meeting in consultation room
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
	| Claimant | Interpreter | auto_vw.interpreter_07@hearings.reform.hmcts.net | 
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
	And all participants have joined the hearing waiting room with SkipToWaitingRoom
	When the judge selects Enter consultation room
	And judge invites every participant into the consultation
	And participants accepts the consultation room invitation
	| Role        |
	| Witness     |
	| Interpreter | 
	Then judge participant panel shows consultation room in use
	And every participants leave consultation room
	And everyone signs out