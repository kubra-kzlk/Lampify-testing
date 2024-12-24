Feature: Lamp Control

  Scenario: Turn off the lamp
    Given the lamp is off
    Then the lamp should be off

  Scenario: Adjust lighting when light intensity is below the threshold
    Given the lamp is off
    When the light intensity is below the threshold
    Then the lamp should be on
    And the brightness should be 50
    And the color should be "Cozy Color"

  Scenario: Adjust lighting when light intensity is above the threshold
    Given the lamp is off
    When the light intensity is above the threshold
    Then the lamp should be on
    And the brightness should be 100
    And the color should be "Bright Color"

  Scenario: Set the mood to Cozy
    Given the lamp is off
    When the mood is set to Cozy
    Then the lamp should be on
    And the brightness should be 50
    And the color should be "Cozy Color"

  Scenario: Set the mood to Bright
    Given the lamp is off
    When the mood is set to Bright
    Then the lamp should be on
    And the brightness should be 100
    And the color should be "Bright Color"

  Scenario: Toggle the lamp on
    Given the lamp is off
    When the lamp is toggled
    Then the lamp should be on

  Scenario: Toggle the lamp off
    Given the lamp is on
    When the lamp is toggled
    Then the lamp should be off

  Scenario: Check safe mode
    Given the lamp is off
    When the lamp is toggled
    Then the lamp should be in safe mode