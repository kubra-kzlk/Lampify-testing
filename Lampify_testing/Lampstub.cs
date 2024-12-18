using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{ //A stub is a simple implementation of an interface that allows you to control the behavior of the object during testing.
  //LampStub would implement the ILamp interface and provide basic functionality for testing purposes.
    public class LampStub : ILamp
    {
        private bool isOn = false;
        private int brightness = 0;
        private string color = "White"; // Default color

        public bool IsOn
        {
            get { return isOn; }
        }
        public int Brightness
        {
            get { return brightness; }
        }
        public string Color
        {
            get { return color; }
        }
        public void TurnOn()
        {
            isOn = true;
            Console.WriteLine("Lamp is turned ON.");
        }
        public void TurnOff()
        {
            isOn = false;
            Console.WriteLine("Lamp is turned OFF.");
        }
        public void SetBrightness(int brightness)
        {
            this.brightness = brightness;
            Console.WriteLine($"Brightness set to {brightness}.");
        }
        public void SetColor(string color)
        {
            this.color = color;
            Console.WriteLine($"Color set to {color}.");
        }
    }
}
