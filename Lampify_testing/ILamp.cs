using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{//interface vr lamp
    public interface ILamp
    {
        public void TurnOn();

        public void TurnOff();

        public void SetBrightness(int brightness);

        public void SetColor(string color);

        public bool IsOn { get; }
        int Brightness { get; }
    }
}
