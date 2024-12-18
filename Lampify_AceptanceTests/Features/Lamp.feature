Feature: Lamp Failure Scenarios

  Scenario: Lamp fails to turn on
    Given the lamp is off
    When I try to turn on the lamp
    Then the lamp should be off
    And an error message should be displayed

  Scenario: Lamp fails to adjust brightness
    Given the lamp is on
    When I set the brightness to 100
    Then the brightness should remain at the previous level
    And an error message should be displayed

  Scenario: Lamp fails to change color
    Given the lamp is on
    When I change the color to "red"
    Then the color should remain the same
    And an error message should be displayed