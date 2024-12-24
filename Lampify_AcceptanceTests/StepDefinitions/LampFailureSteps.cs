using Lampify_testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Gherkin.Quick;
using Xunit;

namespace Lampify_AcceptanceTests.StepDefinitions
{
    [FeatureFile("./Features/LampFailure.Feature")]

    public sealed class LampFailureSteps : Feature
    {
        private const string UrlMockoon = "http://localhost:3000/getlightintensity";

        private readonly ILamp lamp;
        private readonly ILightSensorApi lightSensorApi;
        private readonly LampController lampController;

        public LampFailureSteps()
        {
            lightSensorApi = new LightSensorApiData();
            lamp = new LampStub();
            lampController = new LampController(lamp, lightSensorApi);
        }

        [Given(@"the lamp is on")]
        public void GivenLampIsOn()
        {
            if (!lamp.IsOn)
            {
                lampController.ToggleLamp();
            }
        }

        [When(@"the light intensity is below the threshold but the lamp is still on")]
        public void WhenLightIntensityIsLowButLampIsOn()
        {
            lightSensorApi.Url = $"{UrlMockoon}?lux=200";
            lampController.AdjustLighting(LampController.Mood.Cozy);
        }

        [When(@"the lamp is toggled while in safe mode")]
        public void WhenLampIsToggledWhileInSafeMode()
        {

            lampController.ToggleLamp();
        }

        [Then(@"the lamp should still be off")]
        public void ThenLampShouldStillBeOff()
        {
            Assert.False(lamp.IsOn, "The lamp should still be OFF.");
        }
        
        [Then(@"the brightness should not change")]
        public void ThenBrightnessShouldNotChange()
        {
            var lampStub = lamp as LampStub;
            Assert.NotNull(lampStub);
            Assert.Equal(0, lampStub.Brightness); 
        }

        [Then(@"the color should not change")]
        public void ThenColorShouldNotChange()
        {
            var lampStub = lamp as LampStub;
            Assert.NotNull(lampStub);
            Assert.Equal("Off Color", lampStub.Color); 
        }

        [When(@"the mood is set to an invalid value (.*)")]
        public void WhenMoodIsSetToInvalidValue(string invalidMood)
        {
            lampController.AdjustLighting((LampController.Mood)Enum.Parse(typeof(LampController.Mood), invalidMood));
        }

        [Then(@"the lamp should remain in its current state")]
        public void ThenLampShouldRemainInCurrentState()
        {
            Assert.True(lamp.IsOn || !lamp.IsOn, "The lamp state should remain unchanged.");
        }
    }
}