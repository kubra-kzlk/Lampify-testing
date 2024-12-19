Feature: Lamp Failure Scenarios

  Scenario: Lamp fails to turn on
    Given the lamp is off
    When I try to turn on the lamp
    Then the lamp should remain off
    And an error message should be displayed

  Scenario: Lamp fails to respond to commands
    Given the lamp is on
    When I send a command to change the brightness
    Then the lamp should remain at the previous brightness
    And an error message should be displayed