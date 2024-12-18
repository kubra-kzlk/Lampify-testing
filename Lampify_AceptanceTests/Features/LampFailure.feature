Feature: Additional Lamp Failure Scenarios

  Scenario: Lamp fails to toggle state
    Given the lamp is on
    When I toggle the lamp
    Then the lamp should still be on
    And an error message should be displayed

  Scenario: Lamp fails to enter safe mode
    Given the lamp is on
    When I set the lamp to safe mode
    Then the lamp should be off
    And an error message should be displayed