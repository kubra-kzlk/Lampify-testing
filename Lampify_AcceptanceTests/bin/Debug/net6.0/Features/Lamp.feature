Feature: Lamp functionality

  Scenario: Given the lamp is off, When the user tries to toggle the lamp, Then the lamp should be on
    Given the lamp is off
    When the user tries to toggle the lamp
    Then the lamp should be on

  Scenario: Given the lamp is on, When the user sets the mood to Cozy, Then the lamp should have color "Red" and brightness 50
    Given the lamp is on
    When the user sets the mood to Cozy
    Then the lamp should have color "Red" and brightness 50

  Scenario: Given the lamp is on, When the user sets the mood to Angry, Then the lamp should have color "Red" and brightness 100
    Given the lamp is on
    When the user sets the mood to Angry
    Then the lamp should have color "Red" and brightness 100

  Scenario: Given the lamp is on, When the user sets the mood to Bright, Then the lamp should have color "White" and brightness 100
    Given the lamp is on
    When the user sets the mood to Bright
    Then the lamp should have color "White" and brightness 100

  Scenario: Given the lamp is on, When the user tries to apply settings with invalid brightness of -10, Then the system should throw an ArgumentOutOfRangeException
    Given the lamp is on
    When the user tries to apply settings with invalid brightness of -10
    Then the system should throw an ArgumentOutOfRangeException

