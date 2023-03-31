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
	
Scenario: Manage Team Edit User, Delete User, Restore User
	Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"	
	Then the page should be accessible
	When i click on Manage Work Allocation Dashboard
	And I click on Manage Team 
	And I Search for User "auto_aw.CSOTestUser@hearings.reform.hmcts.net"
	Then I confirm User Displayed on the Page 
	And I Pres edit role Icon 
	And I see Edit Role popup window
	And I change the Role from CSO to Administrator
	And I press save 
	Then I see user updated confirmation on the page
	And I Pres edit role Icon
	And I change the Role from Administrator to CSO
	And I press save 
	Then I see user updated confirmation on the page
	Then I delete User
	And I see popup message with warnings
	Then I Click Yes,proceed button
	And I confirm user has been deleted with option to restore
	Then I click restore team member icon
	And I see popup message box with correct warning
	Then I click Yes,proceed button to restore user
	And I confirm user has been restored. 
		
		
		
Scenario: Work Allocation Manage Team and Allocate Hearing 
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
	Then I click on Allocate Hearing
	Then I Select Date Range
	And I press Search
	And I Select Allocate To User "auto_aw.CSOTestUser@hearings.reform.hmcts.net"
	And I Select First and Second Hearing 
	And I click confirm button
	Then I See Hearing have been updated message
		
		
		
		
				
		
	