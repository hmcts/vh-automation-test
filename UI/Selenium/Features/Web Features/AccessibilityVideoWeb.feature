@web
@Smoketest
@Accessibility
Feature: AccessibilityVideoWeb
	In order to ensure video web is accessible to all users
	As a service
	I want to check each page for potential accessibility issues

Scenario: Hearing List page accessibility
Given an individual on the "Hearing List" page
Then assert page should be accessible

Scenario: Waiting Room page accessibility
Given an individual on the "Waiting Room" page
Then assert page should be accessible

Scenario: Introduction page accessibility
Given an individual on the "Introduction" page
Then assert page should be accessible

Scenario: Equipment-Check page accessibility
Given an individual on the "Equipment-Check" page
Then assert page should be accessible

Scenario: Switch-On-Camera-Microphone page accessibility
Given an individual on the "Switch-On-Camera-Microphone" page
Then assert page should be accessible

Scenario: Camera-Working page accessibility
Given an individual on the "Camera-Working" page
Then assert page should be accessible

Scenario: Microphone-Working page accessibility
Given an individual on the "Microphone-Working" page
Then assert page should be accessible

Scenario: See-And-Hear-Video page accessibility
Given an individual on the "See-And-Hear-Video" page
Then assert page should be accessible

Scenario: Hearing-Rules page accessibility
Given an individual on the "Hearing-Rules" page
Then assert page should be accessible

Scenario: Declaration page accessibility
Given an individual on the "Declaration" page
Then assert page should be accessible