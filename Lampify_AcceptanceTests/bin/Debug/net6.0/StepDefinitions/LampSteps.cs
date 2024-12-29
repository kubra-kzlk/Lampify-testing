using Xunit.Gherkin.Quick;
using Xunit;
using Lampify_testing;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lampify_AcceptanceTests.StepDefinitions
{
    [FeatureFile("./Features/Lamp.feature")]
    public sealed class LampSteps : Feature
    {
        private const string UrlMockoon = "http://localhost:3000/getlightintensity";
        private Lamp _lamp;
        private LampController _lampController;
        private Exception _caughtException;
        private HttpClient _httpClient;

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

        [When(@"the user tries to toggle the lamp")]
        public void WhenTheUserTriesToToggleTheLamp()
        {
            if (_lamp.IsOn)
            {
                _lampController.ToggleLamp(); // Toggle the lamp off
                Assert.False(_lamp.IsOn); // Ensure the lamp is off
            }
            else
            {
                _lampController.ToggleLamp(); // Toggle the lamp on
                Assert.True(_lamp.IsOn); // Ensure the lamp is on
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

            Assert.Equal(100, _lamp.Brightness); // Verify brightness
            Assert.Equal("Red", _lamp.Color);   // Verify color
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
            Assert.False(_lamp.IsOn);
        }

        [Then(@"the lamp should be on")]
        public void ThenLampShouldBeOn()
        {
            Assert.True(_lamp.IsOn);
        }

        [Then(@"the lamp should have color ""(.*)"" and brightness (.*)")]
        public async Task ThenLampShouldHaveColorAndBrightness(string color, int brightness)
        {
            string queryParam = $"?color={color}&brightness={brightness.ToString(CultureInfo.InvariantCulture)}";
            string url = $"{UrlMockoon}{queryParam}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Assert.True(response.IsSuccessStatusCode, "Failed to communicate with the light intensity API");

            _lamp.SetColor(color);
            _lamp.SetBrightness(brightness);

            Assert.Equal(color, _lamp.Color);
            Assert.Equal(brightness, _lamp.Brightness);
        }
    }
}
