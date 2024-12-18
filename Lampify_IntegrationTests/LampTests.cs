using System.Globalization;
using Lampify_testing;
using Moq;
using NUnit.Framework;

namespace Lampify_IntegrationTests
{   
    public class LampTests
    {
        private const string UrlMockoon = "http://localhost:3000/getlightintensity";
        private const string UrlMockoonException = "http://localhost:3000/getlightintensity/exception";

        private ILightSensorApi _lightSensorApi;
        private ILamp _lamp;
        private LampController _lampController;

        [SetUp]
        public void Setup()
        {
            // Arrange: Initialize mocks and stubs
            _lamp = new Mock<ILamp>().Object;
            _lightSensorApi = new Mock<ILightSensorApi>().Object;

            var mockLightSensor = new Mock<ILightSensorApi>();
            mockLightSensor.SetupSequence(api => api.GetLightIntensity())
                .Returns(300) // Simulate a dark environment
                .Throws(new Exception("Sensor failure"));

            _lightSensorApi = mockLightSensor.Object;
            _lampController = new LampController(_lamp, _lightSensorApi);
        }

        [Test]
        public void LampTurnsOnInDarkEnvironment()
        {
            // Arrange
            var mockLightSensor = new Mock<ILightSensorApi>();
            mockLightSensor.Setup(api => api.GetLightIntensity()).Returns(300);
            _lightSensorApi = mockLightSensor.Object;
            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy);

            // Assert
            Assert.IsTrue(_lamp.IsOn, "The lamp should be ON in a dark environment.");
        }

        [Test]
        public void LampStaysOffInBrightEnvironment()
        {
            // Arrange
            var mockLightSensor = new Mock<ILightSensorApi>();
            mockLightSensor.Setup(api => api.GetLightIntensity()).Returns(600);
            _lightSensorApi = mockLightSensor.Object;
            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy);

            // Assert
            Assert.IsFalse(_lamp.IsOn, "The lamp should remain OFF in a bright environment.");
        }

        [Test]
        public void LampTurnsOffWhenMoodIsDark()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();
            mockLamp.Setup(lamp => lamp.IsOn).Returns(true);
            _lamp = mockLamp.Object;
            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            _lampController.AdjustLighting(LampController.Mood.Dark);

            // Assert
            mockLamp.Verify(lamp => lamp.TurnOff(), Times.Once, "The lamp should be turned OFF when the mood is Dark.");
        }

        [Test]
        public void LampRemainsInPreviousStateWhenSensorFails()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();
            mockLamp.Setup(lamp => lamp.IsOn).Returns(true);
            _lamp = mockLamp.Object;

            var mockLightSensor = new Mock<ILightSensorApi>();
            mockLightSensor.SetupSequence(api => api.GetLightIntensity())
                .Returns(300)
                .Throws(new Exception("Sensor failure"));
            _lightSensorApi = mockLightSensor.Object;

            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy); // First call succeeds
            _lampController.AdjustLighting(LampController.Mood.Cozy); // Second call fails

            // Assert
            Assert.IsTrue(_lamp.IsOn, "The lamp should remain ON despite the sensor failure.");
        }

        [Test]
        public void LampEntersSafeModeAfterMultipleFailures()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();
            mockLamp.Setup(lamp => lamp.TurnOff()).Verifiable();
            _lamp = mockLamp.Object;

            var mockLightSensor = new Mock<ILightSensorApi>();
            mockLightSensor.Setup(api => api.GetLightIntensity()).Throws(new Exception("Sensor failure"));
            _lightSensorApi = mockLightSensor.Object;

            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            for (int i = 0; i < 4; i++)
            {
                _lampController.AdjustLighting(LampController.Mood.Cozy);
            }

            // Assert
            mockLamp.Verify(lamp => lamp.TurnOff(), Times.AtLeastOnce, "The lamp should enter Safe Mode after multiple errors.");
        }

        [Test]
        public void LampResetsAfterValidInputFollowingSafeMode()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();
            mockLamp.Setup(lamp => lamp.IsOn).Returns(false);
            _lamp = mockLamp.Object;

            var mockLightSensor = new Mock<ILightSensorApi>();
            mockLightSensor.SetupSequence(api => api.GetLightIntensity())
                .Throws(new Exception("Sensor failure"))
                .Throws(new Exception("Sensor failure"))
                .Returns(600); // Valid input after failures

            _lightSensorApi = mockLightSensor.Object;

            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            for (int i = 0; i < 3; i++)
            {
                _lampController.AdjustLighting(LampController.Mood.Cozy);
            }

            // Assert
            Assert.IsFalse(_lamp.IsOn, "The lamp should remain OFF after entering Safe Mode and receiving valid input.");
        }
    }
}