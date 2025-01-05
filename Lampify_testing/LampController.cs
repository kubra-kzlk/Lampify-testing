using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{// controller bvt logica vr h aansturen vd lamp obvd lichtsterkte en mood, die je kan testen, Verantwoordelijk voor de bediening van de lamp, bv het in- en uitschakelen.
    public class LampController
    {
        private readonly ILightSensorApi _lightSensorApi; //DI, api vr lichtsterkte
        private readonly ILamp _lamp;

        private const int MaxErrorCount = 3;
        private int _errorCount = 0;
        public bool IsOn
        {
            get { return _lamp != null; }
        }

        public enum Mood
        {
            Cozy, // Zet de helderheid op 50 en de kleur op rood
            Angry, //Zet de helderheid op 100 en de kleur op rood
            Bright, //Zet de helderheid op 100 en de kleur op wit
            Dark //Zet de lamp uit als deze aan is
        }

        public LampController(ILamp lamp, ILightSensorApi lightSensorApi)
        {
            _lamp = lamp;
            _lightSensorApi = lightSensorApi;
        }

        public void ToggleLamp()//zet de lamp aan als deze uit is, of uit als deze aan is
        {
            {
                if (_lamp.IsOn)
                {
                    _lamp.TurnOff();
                    Console.WriteLine($"[{DateTime.Now}] Lamp turned OFF."); //loggen van aan- /uitzetten lamp
                }
                else
                {
                    _lamp.TurnOn();
                    Console.WriteLine($"[{DateTime.Now}] Lamp turned ON.");
                }
            }
        }
        public void ApplySettings(int brightness, string color)
        {
            if (!_lamp.IsOn)
            {
                throw new InvalidOperationException("Cannot apply settings when the lamp is off.");
            }

            try
            {
                // Validate the brightness range (0 to 500)
                if (brightness < 0 || brightness > 500)
                {
                    Console.WriteLine("Invalid brightness detected. Throwing exception.");
                    throw new ArgumentOutOfRangeException(nameof(brightness), "Brightness must be between 0 and 500.");
                }

                // If brightness is valid, proceed with setting the brightness and color
                _lamp.SetBrightness(brightness);
                _lamp.SetColor(color);

                Console.WriteLine($"[{DateTime.Now}] Settings applied: Brightness={brightness}, Color={color}"); // Log the applied settings
                _errorCount = 0; // Reset error count on success
            }
            catch (Exception ex)
            {
                _errorCount++;
                Console.WriteLine($"[{DateTime.Now}] Error applying settings: {ex.Message}. Error count: {_errorCount}"); // Log error

                if (_errorCount >= MaxErrorCount)
                {
                    EnterSafeMode(); // Enter safe mode if too many errors occur
                }

                throw; // Re-throw the exception to ensure it is caught by the unit test
            }
        }


        private void EnterSafeMode()
        {//Wanneer de lamp naar de veilige modus gaat, wordt dit gelogd
            Console.WriteLine($"[{DateTime.Now}] Entering Safe Mode: Lamp is turned OFF due to repeated errors.");
            _lamp.TurnOff();
        }

        //lamp ovb mood instellen
        public void AdjustLighting(Mood mood)
        {
            try
            {
                int currentLux = _lightSensorApi.GetLightIntensity(); // Get current light intensity
                Console.WriteLine($"Current light intensity: {currentLux} lux");
                // Reset error count on successful reading
                _errorCount = 0;
                // Check if the current light intensity is below the threshold
                if (currentLux < 500)
                {
                    ToggleLamp(); // Turn on the lamp if it's too dark
                }
                // Adjust settings based on the specified mood
                switch (mood)
                {
                    case Mood.Cozy:
                        ApplySettings(50, "Red"); // Set brightness and color for a cozy atmosphere
                        break;
                    case Mood.Angry:
                        ApplySettings(100, "Red"); // Set brightness and color for an angry atmosphere
                        break;
                    case Mood.Bright:
                        ApplySettings(100, "White"); // Set brightness and color for a bright atmosphere
                        break;
                    case Mood.Dark:
                        if (_lamp.IsOn)
                        {
                            ToggleLamp(); // Turn off the lamp if it's dark
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mood), "Invalid mood specified.");
                }
            }
            catch (Exception ex)
            {
                _errorCount++;
                Console.WriteLine($"[{DateTime.Now}] Error adjusting lighting: {ex.Message}. Error count: {_errorCount}");
                if (_errorCount >= MaxErrorCount)
                {
                    EnterSafeMode(); // Enter safe mode if too many errors occur
                }
                throw;
            }
        }

        public void TurnOn()
        {
            _lamp.TurnOn();
            Console.WriteLine("Lamp turned ON.");
        }

        public void TurnOff()
        {
            _lamp.TurnOff();
            Console.WriteLine("Lamp turned OFF.");
        }

        public void SetBrightness(int brightness)
        {
            _lamp.SetBrightness(brightness);
            Console.WriteLine($"Lamp brightness set to {brightness}.");
        }

        public void SetColor(string color)
        {
            _lamp.SetColor(color);
            Console.WriteLine($"Lamp color set to {color}.");
        }
    }
}