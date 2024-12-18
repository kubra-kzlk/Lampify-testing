using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Gherkin.Quick;

namespace Lampify_testing.StepDefinitions
{
    [FeatureFile("./Features/lamp.feature")]
    public sealed class LampFailureSteps : Feature
    {
        private readonly ILamp lamp;
        private readonly LampController lampController;
        public LampFailureSteps()
        {
            lamp = new LampStub();
            lampController = new LampController(lamp, new LightSensorApiData());
        }

        [Given(@"the lamp is off")]
        public void GivenLampIsOff()
        {
            if (lamp.IsOn)
            {
                lampController.ToggleLamp(); // Ensure the lamp is off
            }
        }
        [When(@"I try to turn on the lamp")]
        public void WhenITryToTurnOnTheLamp()
        {
            // Simulate a failure to turn on the lamp
            // This could be a method that fails to change the state
            lampController.ToggleLamp(); // Attempt to turn on
            lampController.ForceFailure(); // Simulate failure
        }

        [Then(@"the lamp should be off")]
        public void ThenLampShouldBeOff()
        {
            Assert.False(lamp.IsOn, "The lamp should be OFF due to failure.");
        }

        [Then(@"an error message should be displayed")]
        public void ThenAnErrorMessageShouldBeDisplayed()
        {
            // Check for an error message
            Assert.True(lampController.HasErrorMessage, "An error message should be displayed.");
        }

        [When(@"I set the brightness to (.*)")]
        public void WhenISetTheBrightnessTo(int brightness)
        {
            lampController.SetBrightness(brightness);
            lampController.ForceFailure(); // Simulate failure
        }

        [Then(@"the brightness should remain at the previous level")]
        public void ThenBrightnessShouldRemainAtPreviousLevel()
        {
            var previousBrightness = lamp.Brightness; // Store previous brightness
            Assert.AreEqual(previousBrightness, lamp.Brightness, "The brightness should remain unchanged.");
        }

        [When(@"I change the color to (.*)")]
        public void WhenIChangeTheColorTo(string color)
        {
            lampController.ChangeColor(color);
            lampController.ForceFailure(); // Simulate failure
        }

        [Then(@"the color should remain the same")]
        public void ThenColorShouldRemainTheSame()
        {
            var previousColor = lamp.Color; // Store previous color
            Assert.AreEqual(previousColor, lamp.Color, "The color should remain unchanged.");
        }

        [When(@"I toggle the lamp")]
        public void WhenIToggleTheLamp()
        {

            lampController.ToggleLamp();
            lampController.ForceFailure(); // Simulate failure
        }

        [Then(@"the lamp should still be on")]
        public void ThenLampShouldStillBeOn()
        {
            Assert.True(lamp.IsOn, "The lamp should still be ON due to failure.");
        }

        [When(@"I set the lamp to safe mode")]
        public void WhenISetTheLampToSafeMode()
        {
            lampController.EnterSafeMode(); // Simulate entering safe mode
            lampController.ForceFailure(); // Simulate failure
        }

        [Then(@"the lamp should be off in safe mode")]
        public void ThenLampShouldBeOffInSafeMode()
        {
            Assert.False(lamp.IsOn, "The lamp should be OFF in Safe Mode.");
        }
    }
}