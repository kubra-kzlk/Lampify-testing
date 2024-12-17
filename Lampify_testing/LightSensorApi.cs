using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lampify_testing
{
    public class LightSensorApi : ILightSensorApi
    {
        private Random _random = new Random();
        public int GetLightIntensity()
        {
            // Simuleer het ophalen van de lichtsterkte door random lux te genereren
            return _random.Next(0, 1001); 
        }

        Task<int> ILightSensorApi.GetLightIntensity()
        {
            throw new NotImplementedException();
        }
    }
}
