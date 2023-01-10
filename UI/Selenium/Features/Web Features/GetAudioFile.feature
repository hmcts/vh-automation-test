@web
@DeviceTest

Feature: GetAudioFile

A short summary of the feature
	
@ignore
Scenario: Get audio file for main Hearing and Interpreter room
   
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
	| Party     | Role               | Id                                  |
	| Claimant  | Litigant in person | auto_vw.individual_05@hearings.reform.hmcts.net    |
	| Claimant  | Representative     | auto_vw.representative_01@hearings.reform.hmcts.net |

	And With video Access points details
	| Display Name | Advocate |
	|              |          |
	And I set any other information
	| Record Hearing | Other information   |
	| True           | This is a test info |
	And I book the hearing
	Then A hearing should be created
	And I log off
	And all participants log in to video web
	And all participants have joined the hearing waiting room with SkipToWaitingRoom
	And the judge starts the hearing
	And the judge checks that all participants have joined the hearing room 
	Then the judge closes the hearing
	And everyone signs out
	Given I open a new browser and log into admin web as "auto_aw.videohearingsofficer_12@hearings.reform.hmcts.net"
	And Progress to the Get Audio File page
	When I search for the audio recording by case number
	Then the audio recording link for main hearing and for interpreter VMR can be retrieved
