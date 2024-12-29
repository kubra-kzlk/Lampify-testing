using Xunit;
using Xunit.Gherkin.Quick;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Lampify_testing;

namespace Lampify_AcceptanceTests.StepDefinitions
{
    [FeatureFile("./Features/Lamp.feature")]
    public sealed class LampSteps : Feature
    {
        private const string UrlMockoon = "http://localhost:3000/getlightintensity";
        private readonly LampController _lampController;
        private readonly Lamp _lamp;
        private readonly HttpClient _httpClient;
        private Exception _caughtException;

        public LampSteps()
        {
            _lamp = new Lamp();
            _lampController = new LampController(_lamp, new LightSensorApi(UrlMockoon));
            _httpClient = new HttpClient();
        }

        [Given(@"the lamp is off")]
        public void GivenTheLampIsOff()
        {
            _lamp.TurnOff();
        }

        [Given(@"the lamp is on")]
        public void GivenTheLampIsOn()
        {
            _lamp.TurnOn();
        }

        [When(@"the user toggles the lamp")]
        public void WhenTheUserTogglesTheLamp()
        {
            _lampController.ToggleLamp();
        }

        [When(@"the user applies settings with brightness (\d+) and color ""(.*)""")]
        public void WhenTheUserAppliesSettings(int brightness, string color)
        {
            _lampController.ApplySettings(brightness, color);
        }

        [When(@"the user tries to apply settings with invalid brightness of (-?\d+)")]
        public void WhenTheUserTriesToApplyInvalidBrightness(int brightness)
        {
            _caughtException = Record.Exception(() => _lampController.ApplySettings(brightness, "White"));
        }

        [Then(@"the system should throw an ArgumentOutOfRangeException")]
        public void ThenSystemShouldThrowArgumentOutOfRangeException()
        {
            Assert.NotNull(_caughtException);
            Assert.IsType<ArgumentOutOfRangeException>(_caughtException);
        }

        [Then(@"the lamp should be on")]
        public void ThenTheLampShouldBeOn()
        {
            Assert.True(_lamp.IsOn);
        }

        [Then(@"the lamp should have brightness (\d+) and color ""(.*)""")]
        public void ThenTheLampShouldHaveBrightnessAndColor(int brightness, string color)
        {
            Assert.Equal(brightness, _lamp.Brightness);
            Assert.Equal(color, _lamp.Color);
        }

        [Given(@"the light intensity is below 500")]
        public async Task GivenTheLightIntensityIsBelow500()
        {
            var response = await _httpClient.GetAsync(UrlMockoon);
            var content = await response.Content.ReadAsStringAsync();

            Assert.True(int.TryParse(content, out int lux));
            Assert.True(lux < 500, "Mockoon should return a value below 500.");
        }

        [When(@"the user sets the mood to (.*)")]
        public void WhenTheUserSetsTheMoodTo(string mood)
        {
            if (Enum.TryParse(mood, out LampController.Mood parsedMood))
            {
                _lampController.AdjustLighting(parsedMood);
            }
            else
            {
                throw new ArgumentException("Invalid mood specified.");
            }
        }

        [Then(@"the lamp should be off")]
        public void ThenTheLampShouldBeOff()
        {
            Assert.False(_lamp.IsOn);
        }

        [Given(@"the user applies invalid settings 3 times")]
        public void GivenTheUserAppliesInvalidSettingsThreeTimes()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _lampController.ApplySettings(-1, "Invalid");
                }
                catch
                {
                    // Ignore exception
                }
            }
        }

        [Then(@"the lamp should turn off and enter safe mode")]
        public void ThenTheLampShouldTurnOffAndEnterSafeMode()
        {
            Assert.False(_lamp.IsOn);
        }
    }
}
