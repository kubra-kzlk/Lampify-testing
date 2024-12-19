using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Gherkin.Quick;
using Xunit;

namespace Lampify_testing.StepDefinitions
{
    [FeatureFile("./Features/lamp.feature")]
    public sealed class LampFailureSteps : Feature
    {
        private const string UrlMockoon = "http://localhost:3000/getlightintensity";
        private const string UrlMockoonException = "http://localhost:3000/getlightintensity/exception";

        private readonly ILamp lamp = null;
        private readonly ILightSensorApi lightSensorApi = null;
        private readonly LampController lampController;
        public LampFailureSteps()
        {
            lightSensorApi = new LightSensorApiData();
            lamp = new LampStub();
            lampController = new LampController(lamp, lightSensorApi);
        }

        [Given(@"the lamp is off")]
        public void GivenTheLampIsOff()
        {
            if (lamp.IsOn)
            {
                lampController.ToggleLamp();
            }
        }
        [When(@"an exception occurs in the light sensor API")]
        public void WhenExceptionOccursInLightSensorApi()
        {
            lightSensorApi.Url = UrlMockoonException;
            try
            {
                lampController.AdjustLighting(LampController.Mood.Cozy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }
        }

        [Then(@"the lamp should remain off")]
        public void ThenLampShouldRemainOff()
        {
            Assert.False(lamp.IsOn, "The lamp should remain OFF due to API failure.");
        }

        [When(@"the light intensity API returns invalid data")]
        public void WhenLightIntensityApiReturnsInvalidData()
        {
            lightSensorApi.Url = $"{UrlMockoon}?lux=invalid";
            try
            {
                lampController.AdjustLighting(LampController.Mood.Cozy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }
        }

        [Then(@"the lamp should enter safe mode")]
        public void ThenLampShouldEnterSafeMode()
        {
            var lampStub = lamp as LampStub;
            Assert.False(lampStub?.IsOn ?? true, "The lamp should be OFF in Safe Mode.");
            Console.WriteLine("Lamp is in Safe Mode due to invalid data.");
        }

    }
}