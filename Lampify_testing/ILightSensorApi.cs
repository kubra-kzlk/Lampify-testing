using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{// interface vr ophalen lichtsterkte/ fetching light intensity
    public interface ILightSensorApi
    {
        string Url { get; set; } // Property to set the URL for the API
        int GetLightIntensity(); // Method to fetch light intensity in lux
    }
}
