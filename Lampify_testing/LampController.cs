using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{// Verantwoordelijk voor de bediening van de lamp, bv het in- en uitschakelen.
    public class LampController: ILamp
    {
        private readonly Lamp _lamp;
        private int _errorCount = 0;
        private const int MaxErrorCount = 3;

        public LampController(Lamp lamp)
        {
            _lamp = lamp;
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
        public void ApplySettings(int brightness, string color)//past helderheid en kleur aan, int helderheid, string kleur
        {//error behandeling: gebruik van exception
            try
            {
                _lamp.SetBrightness(brightness);
                _lamp.SetColor(color);
                Console.WriteLine($"[{DateTime.Now}] Settings applied: Brightness={brightness}, Color={color}");//loggen v aanpassing v instelling
                _errorCount = 0; // Reset error count on success
            }
            catch (Exception ex)
            {
                _errorCount++;
                Console.WriteLine($"[{DateTime.Now}] Error applying settings: {ex.Message}. Error count: {_errorCount}");//logging: error bij instellen van helderheid/kleur

                if (_errorCount >= MaxErrorCount)
                {
                    EnterSafeMode();
                }

                throw;
            }
        }

        private void EnterSafeMode()
        {//Wanneer de lamp naar de veilige modus gaat, wordt dit gelogd
            Console.WriteLine($"[{DateTime.Now}] Entering Safe Mode: Lamp is turned OFF due to repeated errors.");
            _lamp.TurnOff();
        }

        // Implementatie van ILampApi
        public bool GetLampStatus() => _lamp.IsOn;
        public int GetBrightness() => _lamp.Brightness;
        public string GetColor() => _lamp.Color;
    }
}