Feature: VideoOff

As a participant of a Hearing
When I disable the Video option
The judge shouldn't be able to see me

@web
Scenario: Participant switched off Video in Waiting Room
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
	| Party    | Role               | Id                                              | Video Off |
	| Claimant | Litigant in person | auto_vw.individual_05@hearings.reform.hmcts.net | true      |
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
	And the judge starts the hearing
	When participants switch off the video
	Then the judge sees that participants video has been switched off
	And the judge closes the hearing
	And everyone signs out
	
