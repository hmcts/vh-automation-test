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
	When I click on Manage Work Allocation Dashboard
	And I click on Upload CVS workhours
	Then file is uploaded successfully Working Hours
	And Team working hours uploaded successfully 

Scenario: Upload Non Availability hours
	Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"	
	Then the page should be accessible
	When I click on Manage Work Allocation Dashboard
	Given I click on Upload Workhours and non Availability
	When I click on Upload non Availability hours
	Then file is uploaded successfully non availability hours
	
Scenario: Edit Working Hours 
	Given I log in as "auto_aw.videohearingsofficer_02@hearings.reform.hmcts.net"  
	Then the page should be accessible
	When I click on Manage Work Allocation Dashboard
	And I click on Upload CVS workhours
	Then file is uploaded successfully Working Hours
	When I click on Upload non Availability hours
    Given I click on Edit Working hours and non availability
	And I Click on Edit working hours button
	And Search team members
    When I click on Manage Team
	And Search for team member


				