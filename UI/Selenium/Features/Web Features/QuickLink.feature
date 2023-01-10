@web
@DeviceTest
Feature: Join Hearing By Quick Link

@ignore	
Scenario: Get Hearing lists and join Hearing by Quick Link
	Given I log in video url as "auto_aw.videohearingsofficer_01@hearings.reform.hmcts.net"
	When selects hearing venue in the venue list
	And I click on copy hearing id to clipboard
	Then Hearing id should be copied
	When I click on copy joining by phone details to clipboard
	Then phone details should be copied
	When I click on link to join by Quicklink details to clipboard
	Then I should able to open quicklink on new browser
	When I want to join hearing with details
		| Full Name       |
		| Michael Jackson |
	Then I click on signintoHearing
	And I get ready for the hearing
	And I confirm equipment is working
	And I make sure camera and microphone switched on
	And I continue to watch the video
	And Testing your equipment
	And Checking was your camera working
	And Checking was your microphone working
	And Checking were the image and sound clear
	And I agree to court rules
	And I confirm declaration	