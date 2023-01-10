@web
@DeviceTest
Feature: WaitingRoom

Check all waiting room statuses are correct
	
@ignore
Scenario: Waiting Room Statuses
Given I log in as "auto_aw.videohearingsofficer_11@hearings.reform.hmcts.net"
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
	| Party     | Role               | Id                                                  |
	| Claimant  | Litigant in person | auto_vw.individual_05@hearings.reform.hmcts.net     |
	| Claimant  | Representative     | auto_vw.representative_01@hearings.reform.hmcts.net |
	| Defendant | Litigant in person | auto_vw.individual_06@hearings.reform.hmcts.net     |

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
	And the judge signs into the hearing
	Then assert the judge sees the correct status for each participant as NOT SIGNED IN
	And assert the judge and participant's status change as each participant join the waiting room
	And the judge starts the hearing and checks that all participants have joined the hearing room
	When the judge pauses the hearing
	And each have their status as CONNECTED and Available on judge's and participants screen respectively
	And status should show Disconnected and Unavailable for judge and participants respectively when a participant logs off or close browser