Feature: Lamp Control

  Scenario: Turn on the lamp
    Given the lamp is off
    When the lamp is toggled
    Then the lamp should be on

  Scenario: Turn off the lamp
    Given the lamp is on
    When the lamp is toggled
    Then the lamp should be off

  Scenario: Adjust lighting based on low light intensity
    Given the lamp is off
    When the light intensity is below the threshold
    Then the lamp should be on
    And the brightness should be 50
    And the color should be "Warm White"

  Scenario: Adjust lighting based on high light intensity
    Given the lamp is off
    When the light intensity is above the threshold
    Then the lamp should be on
    And the brightness should be 100
    And the color should be "Cool White"

  Scenario: Set the mood of the lamp
    Given the lamp is off
    When the mood is set to "Cozy"
    Then the lamp should be on
    And the brightness should be 50
    And the color should be "Warm White"

  Scenario: Set the mood of the lamp to bright
    Given the lamp is off
    When the mood is set to "Bright"
    Then the lamp should be on
    And the brightness should be 100
    And the color should be "Cool White"

  Scenario: Check lamp in safe mode
    Given the lamp is on
    When the lamp is toggled
    Then the lamp should be in safe mode