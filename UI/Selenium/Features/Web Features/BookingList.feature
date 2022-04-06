Feature: BookingList

This test runs an a basic e2e test for book a hearing
And then it checks booking list contains newly created hearing

@web
Scenario: Check booking list contains expected values
	Given I have a booked hearing
	When I navigate to booking list page
	And the VHO scrolls to the hearing
	Then The booking should contain expected values

@web
Scenario: VHO Bookings Search by Case Number
	Given I have a booked hearing
	When I navigate to booking list page
	And the VHO search for the booking by case number
	Then the booking is retrieved
	And VHO selects booking
	And the VHO is on the Booking Details page

@web
Scenario: Cancel a future hearing
	Given I have booked a hearing in next 60 minutes
	When I navigate to booking list page
	And the VHO search for the booking by case number
	Then the booking is retrieved
	And VHO selects booking
	And the VHO is on the Booking Details page
	When the VHO cancels the hearing for the reason 'Judge decision'
	Then the VHO sees the hearing is cancelled

@web
Scenario: VHO Bookings Search by Venue
	Given I have booked a hearing in next 60 minutes
	When I navigate to booking list page
	And the VHO search for the booking by venue 'Birmingham Civil and Family Justice Centre'
	And the VHO scrolls to the hearing
	Then VHO selects booking
	And the VHO is on the Booking Details page