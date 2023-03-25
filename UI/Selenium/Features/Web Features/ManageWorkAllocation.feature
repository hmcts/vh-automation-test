@web
@DeviceTest
@Accessibility
Feature: ManageWorkAllocation
	
	
Scenario: WorkAllocation page accessibility
	Given I'm on the "Work-Allocation" page
	Then the page should be accessible
	
Scenario: Upload Working hours
	Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"	
	Then the page should be accessible
	When i click on Manage Work Allocation Dashboard
	And i click on Upload CVS workhours
	Then file is uploaded successfully Working Hours
	And Team working hours uploaded successfully

Scenario: Upload Non Availability hours
	Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"	
	Then the page should be accessible
	When i click on Manage Work Allocation Dashboard
	Given i click on Upload Workhours and non Availability
	When i click on Upload non Availability hours
	Then file is uploaded successfully non availability hours
	
	Scenario: Work Allocation Manage Team 
		Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"	
		Then the page should be accessible
		When i click on Manage Work Allocation Dashboard
		Then i click manage team 
		And I search for new user
		Then I see no user found message and add new user button
		Then I click add new user 
		Then I see new pop up window - Add a justice user
		Then i fill in all details of new user with correct UK number and valid email address
		And  I save changes 
		And i see save successful message and user details
		Then I delete User
		And I see popup message with warnings
		Then I Click Yes,proceed button
		And I confirm user has been deleted with option to restore
		Then I click restore team member icon
		And I see popup message box with correct warning
		Then I click Yes,proceed button to restore user
		And I confirm user has been restored. 
		
		
		
				
		
	