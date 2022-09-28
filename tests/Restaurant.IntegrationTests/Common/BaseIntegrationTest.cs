using Flurl.Http;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Restaurant.IntegrationTests.Common
{
    [Collection("TestCollection")]
    public class BaseIntegrationTest
    {
        protected static Guid GetIdFromHeader(IFlurlResponse response, string path)
        {
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            responseHeaderValue = responseHeaderValue.ToLowerInvariant();
            path = path.ToLowerInvariant();
            var splitted = responseHeaderValue.Split(path + '/');
            var id = Guid.Parse(splitted[1]);
            return id;
        }
    }
}
