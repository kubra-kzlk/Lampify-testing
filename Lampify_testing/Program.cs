using System;
using System.Threading;
using Lampify_testing;
class Program
{
    static void Main(string[] args)
    {
        ILamp lamp = new LampStub();
        ILightSensorApi lightSensorApi = new LightSensorApiData();
        LampController lampController = new LampController(lamp, lightSensorApi);
        while (true)
        {
            // You need to specify a mood here. For example, using Mood.Cozy.
            lampController.AdjustLighting(LampController.Mood.Cozy); // Adjust the mood as needed
            Thread.Sleep(5000);
        }
    }
}