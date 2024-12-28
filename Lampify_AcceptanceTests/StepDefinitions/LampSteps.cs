using Xunit.Gherkin.Quick;
using Xunit;
using Moq;
using Lampify_testing;
using System;

namespace Lampify_AcceptanceTests.StepDefinitions
{
    [FeatureFile("./Features/Lamp.feature")] // Ensure the path is correct
    public sealed class LampSteps : Feature
    {
        private Mock<ILamp> _lampMock;
        private Mock<ILightSensorApi> _lightSensorApiMock;
        private LampController _lampController;
        private Exception _caughtException;

        public LampSteps()
        {
            _lampMock = new Mock<ILamp>();
            _lightSensorApiMock = new Mock<ILightSensorApi>();
            _lampController = new LampController(_lampMock.Object, _lightSensorApiMock.Object);
        }

        [Given(@"the lamp is off")]
        public void GivenTheLampIsOff()
        {
            _lampMock.Setup(l => l.IsOn).Returns(false);
        }

        [Given(@"the lamp is on")]
        public void GivenTheLampIsOn()
        {
            _lampMock.Setup(l => l.IsOn).Returns(true);
        }

        [When(@"the user tries to toggle the lamp")]
        public void WhenTheUserTriesToToggleTheLamp()
        {
            // If the lamp is off, it should be turned on, otherwise it should be turned off
            if (_lampMock.Object.IsOn)
            {
                // Ensure the lamp is on and toggle it (should turn off)
                _lampMock.Setup(l => l.IsOn).Returns(true); // Lamp is on
                _lampController.ToggleLamp(); // Toggle the lamp
                _lampMock.Verify(l => l.TurnOff(), Times.Once); // Verify TurnOff was called
                _lampMock.Setup(l => l.IsOn).Returns(false); // Update the mock state to off
            }
            else
            {
                // Ensure the lamp is off and toggle it (should turn on)
                _lampMock.Setup(l => l.IsOn).Returns(false); // Lamp is off
                _lampController.ToggleLamp(); // Toggle the lamp
                _lampMock.Verify(l => l.TurnOn(), Times.Once); // Verify TurnOn was called
                _lampMock.Setup(l => l.IsOn).Returns(true); // Update the mock state to on
            }
        }


        [When(@"the user sets the mood to Cozy")]
        public void WhenTheUserSetsTheMoodToCozy()
        {
            _lampController.AdjustLighting(LampController.Mood.Cozy);
        }

        [When(@"the user sets the mood to Angry")]
        public void WhenTheUserSetsTheMoodToAngry()
        {
            _lampController.AdjustLighting(LampController.Mood.Angry);
        }

        [When(@"the user sets the mood to Bright")]
        public void WhenTheUserSetsTheMoodToBright()
        {
            _lampController.AdjustLighting(LampController.Mood.Bright);
        }

        [When(@"the user tries to apply settings with invalid brightness of (.*)")]
        public void WhenTheUserTriesToApplyInvalidBrightness(int invalidBrightness)
        {
            _caughtException = Record.Exception(() =>
            {
                _lampController.ApplySettings(invalidBrightness, "White");
            });
        }

        [Then(@"the system should throw an ArgumentOutOfRangeException")]
        public void ThenSystemShouldThrowArgumentOutOfRangeException()
        {
            Assert.NotNull(_caughtException);
            Assert.IsType<ArgumentOutOfRangeException>(_caughtException);
        }

        [Then(@"the lamp should remain off")]
        public void ThenLampShouldRemainOff()
        {
            _lampMock.Verify(l => l.TurnOff(), Times.Never);
            Assert.False(_lampMock.Object.IsOn);
        }

        [Then(@"the lamp should be on")]
        public void ThenLampShouldBeOn()
        {
            _lampMock.Verify(l => l.TurnOn(), Times.Once); // Ensure TurnOn is called once
            Assert.True(_lampMock.Object.IsOn); // Ensure the lamp is on
        }


        [Then(@"the lamp should have color ""(.*)"" and brightness (.*)")]
        public void ThenLampShouldHaveColorAndBrightness(string color, int brightness)
        {
            _lampMock.Verify(l => l.SetColor(color), Times.Once);
            _lampMock.Verify(l => l.SetBrightness(brightness), Times.Once);
        }
    }
}
