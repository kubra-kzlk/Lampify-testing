using Lampify_testing;
using Moq;
using System;
using Xunit;

namespace LampifyTesting.Tests
{
    public class LampSteps
    {
        private Mock<ILamp> _lampMock;
        private Mock<ILightSensorApi> _lightSensorApiMock;
        private LampController _lampController;

        public LampSteps()
        {
            _lampMock = new Mock<ILamp>();
            _lightSensorApiMock = new Mock<ILightSensorApi>();
            _lampController = new LampController(_lampMock.Object, _lightSensorApiMock.Object);
        }

        // Scenario: Given the lamp is off
        [Fact]
        public void GivenTheLampIsOff()
        {
            // Arrange: Mock the lamp to be off
            _lampMock.Setup(l => l.IsOn).Returns(false);

            // Act: No action needed, just verify the state
            Assert.False(_lampMock.Object.IsOn);
        }

        // Scenario: When the user toggles the lamp
        [Fact]
        public void WhenTheUserTogglesTheLamp()
        {
            // Arrange: Mock the lamp to be off
            _lampMock.Setup(l => l.IsOn).Returns(false);

            // Act: Toggle the lamp
            _lampController.ToggleLamp();

            // Assert: Verify that the lamp was turned on
            _lampMock.Verify(l => l.TurnOn(), Times.Once);
        }

        // Scenario: Then the lamp should be on
        [Fact]
        public void ThenTheLampShouldBeOn()
        {
            // Arrange: Mock the lamp to be off initially
            _lampMock.Setup(l => l.IsOn).Returns(false);  // Lamp is initially off

            // Act: Toggle the lamp
            _lampController.ToggleLamp();

            // Assert: Verify that the lamp was turned on
            _lampMock.Verify(l => l.TurnOn(), Times.Once);  // Ensure TurnOn was called
            _lampMock.Verify(l => l.TurnOff(), Times.Never); // Ensure TurnOff was not called
        }


        // Scenario: Given the lamp is on
        [Fact]
        public void GivenTheLampIsOn()
        {
            // Arrange: Mock the lamp to be on
            _lampMock.Setup(l => l.IsOn).Returns(true);

            // Act: No action needed, just verify the state
            Assert.True(_lampMock.Object.IsOn);
        }

        // Scenario: When the user sets the mood to Cozy
        [Fact]
        public void WhenTheUserSetsTheMoodToCozy()
        {
            // Arrange: Mock the lamp to be on
            _lampMock.Setup(l => l.IsOn).Returns(true);

            // Act: Set the mood to Cozy
            _lampController.AdjustLighting(LampController.Mood.Cozy);

            // Assert: Verify that the lamp settings are applied as per Cozy mood
            _lampMock.Verify(l => l.SetBrightness(50), Times.Once);
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once);
        }

        // Scenario: When the user sets the mood to Angry
        [Fact]
        public void WhenTheUserSetsTheMoodToAngry()
        {
            // Arrange: Mock the lamp to be on
            _lampMock.Setup(l => l.IsOn).Returns(true);

            // Act: Set the mood to Angry
            _lampController.AdjustLighting(LampController.Mood.Angry);

            // Assert: Verify that the lamp settings are applied as per Angry mood
            _lampMock.Verify(l => l.SetBrightness(100), Times.Once);
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once);
        }

        // Scenario: When the user sets the mood to Bright
        [Fact]
        public void WhenTheUserSetsTheMoodToBright()
        {
            // Arrange: Mock the lamp to be on
            _lampMock.Setup(l => l.IsOn).Returns(true);

            // Act: Set the mood to Bright
            _lampController.AdjustLighting(LampController.Mood.Bright);

            // Assert: Verify that the lamp settings are applied as per Bright mood
            _lampMock.Verify(l => l.SetBrightness(100), Times.Once);
            _lampMock.Verify(l => l.SetColor("White"), Times.Once);
        }


    }
}
