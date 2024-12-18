using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lampify_testing
{
    public class LightSensorApiData: ILightSensorApi
    {
        private string url = "http://localhost:3000/getlightintensity";

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public int GetLightIntensity()
        {
            using (var httpClient = new HttpClient())
            {
                var httpRespone = httpClient.GetAsync(url).GetAwaiter().GetResult();
                var response = httpRespone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var lightSensorData = JsonConvert.DeserializeObject<LightSensorData>(response);
                return lightSensorData.Intensity;
            }
        }
    }
}
