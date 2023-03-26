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
	Then I click on Allocate Hearing
	Then I click on Manage Team 
	And I Search for User "auto_aw.CSOTestUser@hearings.reform.hmcts.net"
	Then I confirm User Displayed on the Page 
	Then I Pres edit role Icon 
	And I see Edit Role popup window
	Then I chage the Role from CSO to Administrator Or Administrator to CSO
	And I press save 
	Then I see Sucessfull message displayed on the page 
	 
	
Scenario: Allocate Hearing to User 
		Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"	
		Then the page should be accessible
		When i click on Manage Work Allocation Dashboard
		Then I click on Allocate Hearing
		Then I Select Date Range
		And I press Search
		And I Select Allocate To User "auto_aw.CSOTestUser@hearings.reform.hmcts.net"
		And I Select First and Second Hearing 
		And I click confirm button
		Then I See Hearing have been updated message 
		Then I select manage Team and Delete User and Restore user to unallocate Hearing
		
		
				