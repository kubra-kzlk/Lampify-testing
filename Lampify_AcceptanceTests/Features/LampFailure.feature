Feature: Lamp Control Failure Scenarios

  Scenario: Attempt to adjust lighting when the lamp is on but light intensity is low
    Given the lamp is on
    When the light intensity is below the threshold but the lamp is still on
    Then the lamp should still be on
    And the brightness should not change
    And the color should not change

  Scenario: Toggle the lamp while in safe mode
    Given the lamp is off
    When the lamp is toggled while in safe mode
    Then the lamp should still be off

  Scenario: Set the mood to an invalid value
    Given the lamp is off
    When the mood is set to an invalid value "InvalidMood"
    Then the lamp should remain in its current state