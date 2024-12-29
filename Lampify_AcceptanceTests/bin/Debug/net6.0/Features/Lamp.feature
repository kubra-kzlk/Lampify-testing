Feature: Lamp Functionality
  As a user, I want to control the lamp and adjust its settings based on mood and light intensity.

Scenario: Toggling the lamp
  Given the lamp is off
  When the user toggles the lamp
  Then the lamp should be on

Scenario: Applying valid settings
  Given the lamp is on
  When the user applies settings with brightness 80 and color "Blue"
  Then the lamp should have brightness 80 and color "Blue"

Scenario: Applying invalid settings
  Given the lamp is on
  When the user tries to apply settings with invalid brightness of -10
  Then the system should throw an ArgumentOutOfRangeException

Scenario: Adjusting lighting to Cozy mood
  Given the lamp is on
  And the light intensity is below 500
  When the user sets the mood to Cozy
  Then the lamp should have brightness 50 and color "Red"

Scenario: Adjusting lighting to Angry mood
  Given the lamp is on
  And the light intensity is below 500
  When the user sets the mood to Angry
  Then the lamp should have brightness 100 and color "Red"

Scenario: Adjusting lighting to Bright mood
  Given the lamp is on
  And the light intensity is below 500
  When the user sets the mood to Bright
  Then the lamp should have brightness 100 and color "White"

Scenario: Adjusting lighting to Dark mood
  Given the lamp is on
  When the user sets the mood to Dark
  Then the lamp should be off

Scenario: Entering Safe Mode
  Given the lamp is on
  And the user applies invalid settings 3 times
  When the user applies settings with brightness 101
  Then the lamp should turn off and enter safe mode
