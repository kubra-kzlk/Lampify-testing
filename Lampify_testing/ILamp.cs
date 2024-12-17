using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{//interface vr lamp
    internal interface ILamp
    {
        void TurnOn();

        void TurnOff();

        void SetBrightness(int brightness);

        void SetColor(string color);

        bool IsOn { get; }
    }
}
