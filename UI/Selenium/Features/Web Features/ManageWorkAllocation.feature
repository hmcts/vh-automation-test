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
	When i click on Upload non Availability hours
	Then file is uploaded successfully non availability hours



Scenario: Upload Non Availability hours
	Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"	
	Then the page should be accessible
	When i click on Manage Work Allocation Dashboard
	Given i click on Upload Workhours and non Availability
	When i click on Upload non Availability hours
	Then file is uploaded successfully non availability hours
	
	
			
Scenario: WorkAllocation E2E Test
	Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"	
	Then the page should be accessible
	When i click on Manage Work Allocation Dashboard
	And i click on Upload CVS workhours
	Then file is uploaded successfully Working Hours
	When i click on Upload non Availability hours
	
	Given i click on Edit Working hours and non availability
	And i Click on Edit working hours button
	And Search team menbers
	
	When  i click on Manage Team
	And Search for team member
	
	When i click  Allocate Hearings
	And I search Allocate hearings Date Range
	
	#And  i search case types
	