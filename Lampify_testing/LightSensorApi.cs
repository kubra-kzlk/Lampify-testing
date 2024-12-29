using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lampify_testing
{
    public class LightSensorApi : ILightSensorApi
    {
        private string _urlMockoon;

        public LightSensorApi(string urlMockoon)
        {
            _urlMockoon = urlMockoon;
        }

        public string Url
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public async Task<int> GetLightIntensity()
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_urlMockoon);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch light intensity.");
            }

            string content = await response.Content.ReadAsStringAsync();
            if (!int.TryParse(content, out int intensity))
            {
                throw new Exception("Invalid response from light sensor API. Expected an integer.");
            }
            return intensity;
        }

        int ILightSensorApi.GetLightIntensity()
        {
            return GetLightIntensity().GetAwaiter().GetResult();
        }
    }
}
