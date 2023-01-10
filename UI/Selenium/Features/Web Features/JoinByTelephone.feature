@web
@DeviceTest
Feature: Join By Telephone Number Are Correct
	
@ignore	
Scenario:  Join By Telephone
	Given I log in as "auto_aw.videohearingsofficer_10@hearings.reform.hmcts.net"
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
	| Claimant  | Litigant in person | auto_vw.individual_03@hearings.reform.hmcts.net    |
	| Defendant | Litigant in person | auto_vw.individual_06@hearings.reform.hmcts.net    |
	And With video Access points details
	| Display Name | Advocate |
	|              |          |
	And I set any other information
	| Record Hearing | Other information   |
	|                | This is a test info |
	And I book the hearing
	Then A hearing should be created
	When I navigate to booking list page
	And I search for case number
	And I copy telephone participant link
	Then telephone participant link should be copied	
	
@ignore
Scenario:  Join By Video Participant Link
	Given I log in as "auto_aw.videohearingsofficer_10@hearings.reform.hmcts.net"
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
	| Claimant  | Litigant in person | auto_vw.individual_03@hearings.reform.hmcts.net    |
	| Defendant | Litigant in person | auto_vw.individual_06@hearings.reform.hmcts.net    |
	And With video Access points details
	| Display Name | Advocate |
	|              |          |
	And I set any other information
	| Record Hearing | Other information   |
	|                | This is a test info |
	And I book the hearing
	Then A hearing should be created
	When I navigate to booking list page
	And I search for case number
	And I copy video participant link
	Then video participant link should be copied	