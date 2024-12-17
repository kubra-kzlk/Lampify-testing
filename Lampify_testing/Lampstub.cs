using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{ //A stub is a simple implementation of an interface that allows you to control the behavior of the object during testing.
  //LampStub would implement the ILamp interface and provide basic functionality for testing purposes.
    public class Lampstub : ILamp
    {
        public bool IsOn { get; private set; }
        public int Brightness { get; private set; }
        public string Color { get; private set; }
        public void TurnOn()
        {
            IsOn = true;
            Console.WriteLine("Lamp is turned ON.");
        }

        public void TurnOff()
        {
            IsOn = false;
            Console.WriteLine("Lamp is turned OFF.");
        }

        public void SetBrightness(int brightness)
        {
            Brightness = brightness;
            Console.WriteLine($"Brightness set to {brightness}.");
        }

        public void SetColor(string color)
        {
            Color = color;
            Console.WriteLine($"Color set to {color}.");
        }
    }
}
