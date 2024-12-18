using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{ //Bevat de logica vr aan/uit zetten, aanpassen helderheid en kleur.
    public class Lamp : ILamp
    {
        public bool IsOn => throw new NotImplementedException();

        public void TurnOn() //lamp aanzetten
        {
            throw new NotImplementedException();
        }

        public void TurnOff() //lamp uitzetten
        {
            throw new NotImplementedException();
        }

        public void SetBrightness(int brightness)
        {
            throw new NotImplementedException();
        }

        public void SetColor(string color)
        {
            throw new NotImplementedException();
        }
    }

}
