using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{
    public interface ILightSensorApi
    {
       public int GetLightIntensity(); // Haalt lichtsterkte op in lux
    }
}
