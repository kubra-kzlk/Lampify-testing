Feature: Lamp Controller Functionality

  Scenario: Toggle lamp on and off
    Given the lamp is off
    When the user toggles the lamp
    Then the lamp should be on

  Scenario: Apply settings for Cozy mood
    Given the lamp is on
    When the user sets the mood to Cozy
    Then the lamp brightness should be 50
    And the lamp color should be Red

  Scenario: Apply settings for Angry mood
    Given the lamp is on
    When the user sets the mood to Angry
    Then the lamp brightness should be 100
    And the lamp color should be Red

  Scenario: Apply settings for Bright mood
    Given the lamp is on
    When the user sets the mood to Bright
    Then the lamp brightness should be 100
    And the lamp color should be White


