using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{//interface vr lamp
    internal interface ILamp
    {
        bool GetLampStatus(); // Geeft aan of de lamp aan of uit is
        int GetBrightness();  // Haalt de helderheid van de lamp op
        string GetColor();    // Haalt de kleur van de lamp op
    }
}
