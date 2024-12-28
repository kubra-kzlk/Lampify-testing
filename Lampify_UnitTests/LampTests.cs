using Lampify_testing;
using Moq;

namespace Lampify_UnitTests
{
    [TestClass]
    public class LampTests
    {//AdjustLighting-methode die verschillende beslissingen maakt op basis van de lichtsterkte (lux) en de gewenste Mood
        private Mock<ILightSensorApi> _lightSensorApiMock;
        private Mock<ILamp> _lampMock;
        private LampController _lampController;

        [TestInitialize]
        public void Initialize()
        {
            // Create mocks for the dependencies
            _lightSensorApiMock = new Mock<ILightSensorApi>();
            _lampMock = new Mock<ILamp>();
            // Create the LampController with the mocked dependencies
            _lampController = new LampController(_lampMock.Object, _lightSensorApiMock.Object);
        }

        // Z - Zero: Test when no light is detected
        [TestMethod]
        public void AdjustLighting_TurnsOnLamp_WhenDarkEnvironment()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate dark environment
            _lampMock.Setup(l => l.IsOn).Returns(false); // Mock that the lamp is off initially
            _lampMock.Setup(l => l.TurnOn()).Callback(() =>
            {
                // After turning on the lamp, make sure IsOn returns true.
                _lampMock.Setup(l => l.IsOn).Returns(true);
            }).Verifiable(); // Ensure that TurnOn is called

            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy);

            // Assert
            // Verify that the lamp is turned on
            _lampMock.Verify(l => l.TurnOn(), Times.Once, "The lamp should be turned ON when it's dark.");
        }




        // O - One: Test specific mood settings
        [TestMethod]
        public void AdjustLighting_SetsCozyMood_WhenDarkEnvironment()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate dark environment

            // Initially, the lamp is off
            _lampMock.Setup(l => l.IsOn).Returns(false);

            // Simulate turning the lamp on when ToggleLamp() is called in AdjustLighting
            _lampMock.Setup(l => l.TurnOn()).Callback(() => _lampMock.Setup(l => l.IsOn).Returns(true));

            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy);

            // Assert
            // The lamp should be turned on when the environment is dark
            _lampMock.Verify(l => l.TurnOn(), Times.Once, "The lamp should be turned ON when it's dark.");

            // Verify that the correct settings are applied for the Cozy mood
            _lampMock.Verify(l => l.SetBrightness(50), Times.Once, "Brightness should be set to 50 for Cozy mood.");
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once, "Color should be set to Red for Cozy mood.");
        }

        [TestMethod]
        public void AdjustLighting_SetsAngryMood_WhenDarkEnvironment()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate dark environment

            // Initially, the lamp is off
            _lampMock.Setup(l => l.IsOn).Returns(false);

            // Simulate turning the lamp on when ToggleLamp() is called in AdjustLighting
            _lampMock.Setup(l => l.TurnOn()).Callback(() => _lampMock.Setup(l => l.IsOn).Returns(true));

            // Act
            _lampController.AdjustLighting(LampController.Mood.Angry);

            // Assert
            // The lamp should be turned on when the environment is dark
            _lampMock.Verify(l => l.TurnOn(), Times.Once, "The lamp should be turned ON when it's dark.");

            // Verify that the correct settings are applied for the Angry mood
            _lampMock.Verify(l => l.SetBrightness(100), Times.Once, "Brightness should be set to 100 for Angry mood.");
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once, "Color should be set to Red for Angry mood.");
        }



        // M - Many: Test multiple settings
        [TestMethod]
        public void AdjustLighting_SetsMultipleMoods_Sequentially()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate dark environment

            // Initially, the lamp is off
            _lampMock.Setup(l => l.IsOn).Returns(false);

            // Simulate turning the lamp on when ToggleLamp() is called in AdjustLighting
            _lampMock.Setup(l => l.TurnOn()).Callback(() => _lampMock.Setup(l => l.IsOn).Returns(true));

            // Act - Apply first mood: Cozy
            _lampController.AdjustLighting(LampController.Mood.Cozy);

            // Act - Apply second mood: Bright
            _lampController.AdjustLighting(LampController.Mood.Bright);

            // Assert
            // Verify that the lamp is turned on
            _lampMock.Verify(l => l.TurnOn(), Times.Once, "The lamp should be turned ON when it's dark.");

            // Verify that the correct settings are applied for the Cozy mood
            _lampMock.Verify(l => l.SetBrightness(50), Times.Once, "Brightness should be set to 50 for Cozy mood.");
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once, "Color should be set to Red for Cozy mood.");

            // Verify that the correct settings are applied for the Bright mood
            _lampMock.Verify(l => l.SetBrightness(100), Times.Once, "Brightness should be set to 100 for Bright mood.");
            _lampMock.Verify(l => l.SetColor("White"), Times.Once, "Color should be set to White for Bright mood.");
        }

        // B - Boundary: Test Brightness Limits
        // checks that an exception is thrown when the brightness is set outside the valid range (0 to 100)
        [TestMethod]
        public void ApplySettings_ThrowsException_WhenBrightnessOutOfRange()
        {
            // Arrange
            var brightness = 150; // Invalid brightness (out of range)
            var color = "Red";

            // Ensure the lamp is on before applying settings
            _lampMock.Setup(l => l.IsOn).Returns(true);

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _lampController.ApplySettings(brightness, color));

            // Verify the exception message and parameter name
            Assert.IsTrue(exception.Message.Contains("Brightness must be between 0 and 100."));
            Assert.AreEqual("brightness", exception.ParamName);
        }


        //I - Invalid: Test Invalid Mood
        //checks that an exception is thrown when an invalid mood is passed to the AdjustLighting method
        [TestMethod]
        public void AdjustLighting_ThrowsException_WhenInvalidMood()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate normal light environment

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _lampController.AdjustLighting((LampController.Mood)999)); // Invalid mood (out of range)

            // Update the test to match the complete exception message
            Assert.AreEqual("Invalid mood specified. (Parameter 'mood')", exception.Message); // Expect the full message
            Assert.AreEqual("mood", exception.ParamName); // Verify the parameter name
        }



        //E - Error: Test Error Handling
        // checks that the lamp controller handles errors gracefully when the light sensor fails. You can simulate an error by throwing an exception in the GetLightIntensity method.
        [TestMethod]
        public void AdjustLighting_HandlesError_WhenLightSensorFails()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Throws(new Exception("Sensor failure")); // Simulate sensor failure

            // Act & Assert
            var exception = Assert.ThrowsException<Exception>(() =>
                _lampController.AdjustLighting(LampController.Mood.Cozy));

            // Optionally, verify the exception message
            Assert.AreEqual("Sensor failure", exception.Message);
        }
    }
}