@automated
Feature: Application Users Require a Token to make requests

Background:
	Given following 2 users having
		| UserName      | Password  |
		| admin_123		| admin123 |
		| badmin		| bad_pass	|


@WI10
Scenario: User tries to get a token
	When they request for token
	Then the token result should be
		| UserName  | Got Token |
		| admin_123	| true		|
		| badmin    | false		|