using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{// interface vr ophalen lichtsterkte
    public interface ILightSensorApi
    {
       Task< int> GetLightIntensity(); // Haalt lichtsterkte op in lux
    }
}
