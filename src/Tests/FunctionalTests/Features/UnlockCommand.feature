Feature: Commands.Unlock

@mytag
Scenario: Invalid User Request
	Given the userId is 00000000-0000-0000-0000-000000000000
	And the lockId is 4ab78e29-c12f-4381-a1dd-3e605b959fbf
	When requted for unlock
	Then the result should have status 400
	And the error should contain LockId