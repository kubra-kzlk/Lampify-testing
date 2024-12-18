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
            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy);
            // Assert
            _lampMock.Verify(l => l.TurnOn(), Times.Once, "The lamp should be turned ON when it's dark.");
        }

        // O - One: Test specific mood settings
        [TestMethod]
        public void AdjustLighting_SetsCozyMood_WhenDarkEnvironment()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate dark environment
            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy);
            // Assert
            _lampMock.Verify(l => l.SetBrightness(50), Times.Once, "Brightness should be set to 50 for Cozy mood.");
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once, "Color should be set to Red for Cozy mood.");
        }

        [TestMethod]
        public void AdjustLighting_SetsAngryMood_WhenDarkEnvironment()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate dark environent
            // Act
            _lampController.AdjustLighting(LampController.Mood.Angry);
            // Assert
            _lampMock.Verify(l => l.SetBrightness(100), Times.Once, "Brightness should be set to 100 for Angry mood.");
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once, "Color should be set to Red for Angry mood.");
        }
        // M - Many: Test multiple settings
        [TestMethod]
        public void AdjustLighting_SetsMultipleMoods_Sequentially()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate dark environment
            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy);
            _lampController.AdjustLighting(LampController.Mood.Bright);
            // Assert
            _lampMock.Verify(l => l.SetBrightness(50), Times.Once, "Brightness should be set to 50 for Cozy mood.");
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once, "Color should be set to Red for Cozy mood.");
            _lampMock.Verify(l => l.SetBrightness(100), Times.Once, "Brightness should be set to 100 for Bright mood.");
            _lampMock.Verify(l => l.SetColor("White"), Times.Once, "Color should be set to White for Bright mood.");
        }
        // B - Boundary: Test Brightness Limits
        // checks that an exception is thrown when the brightness is set outside the valid range (0 to 100)
        [TestMethod]
        public void ApplySettings_ThrowsException_WhenBrightnessOutOfRange()
        {
            // Arrange
            var brightness = 150; // Invalid brightness
            var color = "Red";
            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _lampController.ApplySettings(brightness, color));
        }
        //I - Invalid: Test Invalid Mood
        //checks that an exception is thrown when an invalid mood is passed to the AdjustLighting method
        [TestMethod]
        public void AdjustLighting_ThrowsException_WhenInvalidMood()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Returns(300); // Simulate dark environment
            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _lampController.AdjustLighting((LampController.Mood)999)); // Invalid mood
        }

        //E - Error: Test Error Handling
        // checks that the lamp controller handles errors gracefully when the light sensor fails. You can simulate an error by throwing an exception in the GetLightIntensity method.
        [TestMethod]
        public void AdjustLighting_HandlesError_WhenLightSensorFails()
        {
            // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()).Throws(new Exception("Sensor failure")); // Simulate sensor failure
            // Act & Assert
            Assert.ThrowsException<Exception>(() =>
                _lampController.AdjustLighting(LampController.Mood.Cozy)); // Expect an exception due to sensor failure
        }
    }
}