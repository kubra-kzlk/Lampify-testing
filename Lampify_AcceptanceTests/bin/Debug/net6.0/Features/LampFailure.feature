Feature: Lamp Controller Failure Scenarios

  Scenario: Apply settings with invalid brightness
    Given the lamp is on
    When the user tries to apply invalid brightness of 150
    Then the system should throw an ArgumentOutOfRangeException


  Scenario: Apply settings when lamp is off
    Given the lamp is off
    When the user tries to apply settings
    Then the system should throw an InvalidOperationException

  Scenario: Toggle lamp when in safe mode
    Given the lamp is in safe mode
    When the user tries to toggle the lamp
    Then the lamp should remain off



  Scenario: When the lamp is toggled while in safe mode
    Given the lamp is in safe mode
    When the lamp is toggled while in safe mode
    Then the lamp should still be off

  Scenario: When the mood is set to an invalid value
    Given the lamp is on
    When the mood is set to an invalid value "NonExistingMood"
    Then the lamp should remain in its current state
