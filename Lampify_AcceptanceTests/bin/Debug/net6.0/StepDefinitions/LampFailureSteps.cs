using Xunit.Gherkin.Quick;
using Xunit;
using Moq;
using Lampify_testing;
using System;

namespace Lampify_AcceptanceTests.StepDefinitions
{
    [FeatureFile("./Features/LampFailure.feature")]
    public sealed class LampFailureSteps : Feature
    {
        private Mock<ILamp> _lampMock;
        private Mock<ILightSensorApi> _lightSensorApiMock;
        private LampController _lampController;
        private Exception _caughtException;

        public LampFailureSteps()
        {
            _lampMock = new Mock<ILamp>();
            _lightSensorApiMock = new Mock<ILightSensorApi>();
            _lampController = new LampController(_lampMock.Object, _lightSensorApiMock.Object);
        }

        [Given(@"the lamp is on")]
        public void GivenTheLampIsOn()
        {
            _lampMock.Setup(l => l.IsOn).Returns(true);
        }

        [Given(@"the lamp is off")]
        public void GivenTheLampIsOff()
        {
            _lampMock.Setup(l => l.IsOn).Returns(false);
        }

        [Given(@"the lamp is in safe mode")]
        public void GivenTheLampIsInSafeMode()
        {
            _lampMock.Setup(l => l.IsOn).Returns(false);
        }

        [When(@"the user tries to apply invalid brightness of (.*)")]
        public void WhenTheUserTriesToApplyInvalidBrightness(int invalidBrightness)
        {
            _caughtException = Record.Exception(() =>
            {
                _lampController.ApplySettings(invalidBrightness, "White");
            });
        }


        [When(@"the user tries to apply settings")]
        public void WhenTheUserTriesToApplySettings()
        {
            _caughtException = Record.Exception(() =>
            {
                _lampController.ApplySettings(50, "White");
            });
        }

        [When(@"the user tries to toggle the lamp")]
        public void WhenTheUserTriesToToggleTheLamp()
        {
            _lampController.ToggleLamp();
        }

        [When(@"the lamp is toggled while in safe mode")]
        public void WhenTheLampIsToggledWhileInSafeMode()
        {
            _caughtException = Record.Exception(() =>
            {
                _lampController.ToggleLamp();
            });
        }


        [When(@"the mood is set to an invalid value ""(.*)""")]
        public void WhenMoodIsSetToInvalidValue(string invalidMood)
        {
            _caughtException = Record.Exception(() =>
            {
                _lampController.AdjustLighting((LampController.Mood)Enum.Parse(typeof(LampController.Mood), invalidMood));
            });
        }

        [Then(@"the system should throw an ArgumentOutOfRangeException")]
        public void ThenSystemShouldThrowArgumentOutOfRangeException()
        {
            Assert.NotNull(_caughtException);
            Assert.IsType<ArgumentOutOfRangeException>(_caughtException);
        }

        [Then(@"the system should throw an InvalidOperationException")]
        public void ThenSystemShouldThrowInvalidOperationException()
        {
            Assert.NotNull(_caughtException);
            Assert.IsType<InvalidOperationException>(_caughtException);
        }

        [Then(@"the lamp should remain off")]
        public void ThenLampShouldRemainOff()
        {
            _lampMock.Verify(l => l.TurnOff(), Times.Never);
            Assert.False(_lampMock.Object.IsOn);
        }


        [Then(@"the lamp should still be off")]
        public void ThenLampShouldStillBeOff()
        {
            Assert.False(_lampMock.Object.IsOn);
        }

        [Then(@"the lamp should remain in its current state")]
        public void ThenLampShouldRemainInItsCurrentState()
        {
            _lampMock.Verify(l => l.TurnOn(), Times.Never);
            _lampMock.Verify(l => l.TurnOff(), Times.Never);
        }
    }
}
