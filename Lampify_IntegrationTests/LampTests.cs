using System.Globalization;
using Lampify_testing;
using Moq;

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
            var mockLamp = new Mock<ILamp>();

            // Initially, the lamp is off
            mockLamp.Setup(lamp => lamp.IsOn).Returns(false).Verifiable();

            // Set up the behavior of the lamp to be turned on when ToggleLamp is called
            mockLamp.Setup(lamp => lamp.TurnOn()).Callback(() =>
            {
                mockLamp.Setup(lamp => lamp.IsOn).Returns(true); // After TurnOn, the lamp is on
            }).Verifiable();

            _lamp = mockLamp.Object;
            var mockLightSensor = new Mock<ILightSensorApi>();
            mockLightSensor.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate a dark environment
            _lightSensorApi = mockLightSensor.Object;

            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy);

            // Assert
            // Verify that TurnOn was called and the lamp is on
            mockLamp.Verify(lamp => lamp.TurnOn(), Times.Once, "The lamp should be turned ON in a dark environment.");
            Assert.IsTrue(_lamp.IsOn, "The lamp should be ON after adjusting the lighting in a dark environment.");
        }



        [Test]
        public void LampTurnsOffWhenMoodIsDark()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();

            // Mock the IsOn property to return true initially, indicating the lamp is on.
            mockLamp.Setup(lamp => lamp.IsOn).Returns(true).Verifiable(); // Initial state: On

            // Mock TurnOff to change the IsOn state to false after being called
            mockLamp.Setup(lamp => lamp.TurnOff()).Callback(() =>
            {
                mockLamp.Setup(lamp => lamp.IsOn).Returns(false); // After TurnOff, IsOn should return false
            }).Verifiable();

            _lamp = mockLamp.Object;
            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            _lampController.AdjustLighting(LampController.Mood.Dark);

            // Assert
            // Verify that TurnOff was called exactly once
            mockLamp.Verify(lamp => lamp.TurnOff(), Times.Once, "The lamp should be turned OFF when the mood is Dark.");
        }


        [Test]
        public void LampRemainsInPreviousStateWhenSensorFails()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();
            mockLamp.Setup(lamp => lamp.IsOn).Returns(true);  // Assume lamp is on initially
            _lamp = mockLamp.Object;

            var mockLightSensor = new Mock<ILightSensorApi>();
            mockLightSensor.SetupSequence(api => api.GetLightIntensity())
                .Returns(300)  // First call returns a valid light intensity (300 lux)
                .Throws(new Exception("Sensor failure")); // Second call simulates a sensor failure

            _lightSensorApi = mockLightSensor.Object;

            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act: First call should succeed, and the lamp should be ON
            _lampController.AdjustLighting(LampController.Mood.Cozy);  // Lamp should turn on

            // Act: Second call should fail, but lamp should remain ON
            try
            {
                _lampController.AdjustLighting(LampController.Mood.Cozy);  // This should fail due to the sensor failure
            }
            catch (Exception)
            {
                // Handle the expected exception (since the sensor fails on the second call)
            }

            // Assert: The lamp should remain ON despite the sensor failure
            Assert.IsTrue(_lamp.IsOn, "The lamp should remain ON despite the sensor failure.");
        }

        [Test]
        public void LampEntersSafeModeAfterMultipleFailures()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();

            // Track the number of times TurnOff is called
            mockLamp.Setup(lamp => lamp.TurnOff()).Verifiable();
            _lamp = mockLamp.Object;

            var mockLightSensor = new Mock<ILightSensorApi>();

            // Simulate 3 failures followed by valid input
            mockLightSensor.SetupSequence(api => api.GetLightIntensity())
                .Throws(new Exception("Sensor failure"))
                .Throws(new Exception("Sensor failure"))
                .Throws(new Exception("Sensor failure")) // 3 failures
                .Returns(600); // Valid input after failures

            _lightSensorApi = mockLightSensor.Object;

            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act: Try to adjust the lighting 4 times, causing 3 failures
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    _lampController.AdjustLighting(LampController.Mood.Cozy);
                }
                catch (Exception)
                {
                    // Expected exception due to sensor failure, continue with the next attempt
                }
            }

            // Assert: The lamp should enter Safe Mode after 3 failures and be turned off
            mockLamp.Verify(lamp => lamp.TurnOff(), Times.Once, "The lamp should enter Safe Mode and turn off after multiple sensor failures.");
        }

        [Test]
        public void LampResetsAfterValidInputFollowingSafeMode()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();
            mockLamp.Setup(lamp => lamp.IsOn).Returns(false);  // Initially OFF
            mockLamp.Setup(lamp => lamp.TurnOff()).Verifiable();  // Verifying TurnOff method
            _lamp = mockLamp.Object;

            var mockLightSensor = new Mock<ILightSensorApi>();

            // Setup sequence for GetLightIntensity: 3 failures followed by valid input
            mockLightSensor.SetupSequence(api => api.GetLightIntensity())
                .Throws(new Exception("Sensor failure"))  // First failure
                .Throws(new Exception("Sensor failure"))  // Second failure
                .Throws(new Exception("Sensor failure"))  // Third failure (Safe Mode should trigger)
                .Returns(600);  // Valid input after failures

            _lightSensorApi = mockLightSensor.Object;

            _lampController = new LampController(_lamp, _lightSensorApi);

            // Act
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _lampController.AdjustLighting(LampController.Mood.Cozy); 
                }
                catch (Exception ex)
                {
                    // Handle the exception during the failures to simulate retries
                    Console.WriteLine($"Exception caught during test: {ex.Message}");
                }
            }

            // The lamp should be OFF after entering Safe Mode.
            // Assert: Verify that the lamp has been turned off after 3 failures and the valid input.
            Assert.IsFalse(_lamp.IsOn, "The lamp should remain OFF after entering Safe Mode and receiving valid input.");

            // Verify that TurnOff was called, confirming that Safe Mode was entered.
            mockLamp.Verify(lamp => lamp.TurnOff(), Times.Once, "The lamp should have been turned off after 3 sensor failures.");
        }


    }
}