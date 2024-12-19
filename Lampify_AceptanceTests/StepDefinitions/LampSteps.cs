using NUnit.Framework;
using System;
using System.Globalization;
using Xunit.Gherkin.Quick;

namespace Lampify_testing.StepDefinitions
{//technische vertalingen (C# code) van de features
    [FeatureFile("./Features/Lamp.Feature")]
    public sealed class LampSteps : Feature
    {
        private const string UrlMockoon = "http://localhost:3000/getlightintensity";
        private readonly ILamp lamp = null;
        private readonly ILightSensorApi lightSensorApi = null;
        private readonly LampController lampController = null;

        public LampSteps()
        {
            lightSensorApi = new LightSensorApiData();
            lamp = new LampStub();
            lampController = new LampController(lamp, lightSensorApi);
        }

        [Given(@"the lamp is off")]
        public void GivenLampIsOff()
        {
            if (lamp.IsOn)
            {
                lampController.ToggleLamp();
            }
        }

        [When(@"the light intensity is below the threshold")]
        public void WhenLightIntensityIsLow()
        {
            lightSensorApi.Url = $"{UrlMockoon}?lux=200";
            lampController.AdjustLighting(LampController.Mood.Cozy);
        }

        [When(@"the light intensity is above the threshold")]
        public void WhenLightIntensityIsHigh()
        {
            lightSensorApi.Url = $"{UrlMockoon}?lux=800";
            lampController.AdjustLighting(LampController.Mood.Bright);
        }

        [When(@"the mood is set to (.*)")]
        public void WhenMoodIsSet(string mood)
        {
            Enum.TryParse(mood, out LampController.Mood selectedMood);
            lampController.AdjustLighting(selectedMood);
        }

        [Then(@"the lamp should be on")]
        public void ThenLampShouldBeOn()
        {
            Assert.True(lamp.IsOn, "The lamp should be ON.");
        }

        [Then(@"the lamp should be off")]
        public void ThenLampShouldBeOff()
        {
            Assert.False(lamp.IsOn, "The lamp should be OFF.");
        }

        [And(@"the brightness should be (\d+)")]
        public void AndBrightnessShouldBe(int expectedBrightness)
        {
            var lampStub = lamp as LampStub;
            Assert.NotNull(lampStub);
            Assert.AreEqual(expectedBrightness, lampStub.Brightness);
        }

        [And(@"the color should be (.*)")]
        public void AndColorShouldBe(string expectedColor)
        {
            var lampStub = lamp as LampStub;
            Assert.NotNull(lampStub);
            Assert.AreEqual(expectedColor, lampStub.Color);
        }

        [When(@"the lamp is toggled")]
        public void WhenLampIsToggled()
        {
            lampController.ToggleLamp();
        }

        [Then(@"the lamp should be in safe mode")]
        public void ThenLampShouldBeInSafeMode()
        {
            var lampStub = lamp as LampStub;
            Assert.False(lampStub?.IsOn ?? true, "The lamp should be OFF in Safe Mode.");
            Console.WriteLine("Lamp is in Safe Mode.");
        }
    }
}
