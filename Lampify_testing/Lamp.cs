using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{ //Bevat de logica vr aan/uit zetten, aanpassen helderheid en kleur.
    public class Lamp : ILamp
    {
        private bool _isOn; // Backing field for IsOn property.
        private int _brightness;
        private string _color;

        public bool IsOn => _isOn; // Read-only property.

        public int Brightness => _brightness;

        public IEnumerable<char> Color => _color;

        public void TurnOn() // Turn the lamp on.
        {
            _isOn = true;
        }

        public void TurnOff() // Turn the lamp off.
        {
            _isOn = false;
        }

        public void SetBrightness(int brightness)
        {
            if (brightness < 0 || brightness > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(brightness), "Brightness must be between 0 and 100.");
            }

            _brightness = brightness;
        }

        public void SetColor(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                throw new ArgumentException("Color cannot be null or empty.", nameof(color));
            }

            _color = color;
        }
    }

}
