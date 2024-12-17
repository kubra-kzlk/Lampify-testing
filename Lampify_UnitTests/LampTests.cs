using Lampify_testing;
using Moq;

namespace Lampify_UnitTests
{
    [TestClass]
    public class LampTests
    {//AdjustLighting-methode die verschillende beslissingen maakt op basis van de lichtsterkte (lux) en de gewenste Mood
        private Mock<ILightSensorApi> _lightSensorApiMock;
        private Mock<Lamp> _lampMock;
        private LampController _lampController;

        [TestInitialize]
        public void Initialize()
        {
            // Create mocks for the dependencies
            _lightSensorApiMock = new Mock<ILightSensorApi>();
            _lampMock = new Mock<Lamp>();
            // Create the LampController with the mocked dependencies
            _lampController = new LampController(_lampMock.Object, _lightSensorApiMock.Object);
        }

        [TestMethod]
        public void AdjustLighting_SetsCozyMood_WhenDarkEnvironment()
        {//controleren of de lamp wordt ingeschakeld en of de instellingen voor helderheid en kleur correct zijn toegepast (50 voor helderheid en rood voor kleur)
         // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()); // Simuleer donkere omgeving: stellen in dat de lichtsterkte (GetLightIntensity) 300 lux is, wat aangeeft dat de omgeving donker is (d.w.z. minder dan 500 lux)
            // Act
            _lampController.AdjustLighting(LampController.Mood.Cozy); // roepen de AdjustLighting-methode aan met de Mood.Cozy om de lamp in de juiste toestand te zetten
            // Assert
            _lampMock.Verify(l => l.TurnOn(), Times.Once, "De lamp moet ingeschakeld worden.");
            _lampMock.Verify(l => l.SetBrightness(50), Times.Once, "De helderheid moet ingesteld worden op 50 voor de 'Cozy' mood.");
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once, "De kleur moet ingesteld worden op rood voor de 'Cozy' mood.");
        }
        [TestMethod]
        public void AdjustLighting_SetsAngryMood_WhenDarkEnvironment()
        {   // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity());// Simulate dark environment
            // Act
            _lampController.AdjustLighting(LampController.Mood.Angry);
            // Assert
            _lampMock.Verify(l => l.TurnOn(), Times.Once, "The lamp should be turned ON.");
            _lampMock.Verify(l => l.SetBrightness(100), Times.Once, "Brightness should be set to 100 for Angry mood.");
            _lampMock.Verify(l => l.SetColor("Red"), Times.Once, "Color should be set to Red for Angry mood.");
        }

        [TestMethod]
        public void AdjustLighting_SetsBrightMood_WhenDarkEnvironment()
        {  // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()); // Simulate dark environment
            // Act
            _lampController.AdjustLighting(LampController.Mood.Bright);
            // Assert
            _lampMock.Verify(l => l.TurnOn(), Times.Once, "The lamp should be turned ON.");
            _lampMock.Verify(l => l.SetBrightness(100), Times.Once, "Brightness should be set to 100 for Bright mood.");
            _lampMock.Verify(l => l.SetColor("White"), Times.Once, "Color should be set to White for Bright mood.");
        }

        [TestMethod]
        public void AdjustLighting_TurnsOffLamp_WhenDarkMood()
        {  // Arrange
            _lampMock.Setup(l => l.IsOn).Returns(true); // Simulate that the lamp is currently ON
            _lightSensorApiMock.Setup(api => api.GetLightIntensity()); // Simulate dark environment
            // Act
            _lampController.AdjustLighting(LampController.Mood.Dark);
            // Assert
            _lampMock.Verify(l => l.TurnOff(), Times.Once, "The lamp should be turned OFF when mood is Dark.");
        }
        [TestMethod]
        public void AdjustLighting_ThrowsException_WhenInvalidMood()
        {  // Arrange
            _lightSensorApiMock.Setup(api => api.GetLightIntensity());// Simulate dark environment
            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _lampController.AdjustLighting((LampController.Mood)999)); // Invalid mood
        }
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

        [TestMethod]
        public void ApplySettings_ThrowsException_WhenColorIsNull()
        {
            // Arrange
            var brightness = 50;
            string color = null; // Invalid color
            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
                _lampController.ApplySettings(brightness, color));
        }
    }
}