using System.Globalization;
using Lampify_testing;
using Moq;
using NUnit.Framework;

namespace Lampify_IntegrationTests
{   
    public class LampTests
    {
        private const string UrlMockoon = "http://localhost:3000/light/intensity";
        private const string UrlMockoonException = "http://localhost:3000/light/intensity/exception";
        private ILightSensorApi lightSensorApi;
        private ILamp lamp;
        private LampController lampController;

     
    }
}