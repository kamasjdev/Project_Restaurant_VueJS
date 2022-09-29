using Flurl.Http;
using Microsoft.Extensions.Options;
using Restaurant.Api;
using Restaurant.IntegrationTests.Common;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Controllers
{
    public class HealthCheckControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_return_health_check_message()
        {
            var message = _options.Value;

            var response = await _client.Request(Path).GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var content = await response.ResponseMessage.Content.ReadAsStringAsync();
            content.ToLowerInvariant().ShouldBe(message.Name.ToLowerInvariant());
        }

        private readonly IOptions<AppOptions> _options;
        private const string Path = "/api";

        public HealthCheckControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _options = GetService<IOptions<AppOptions>>();
        }
    }
}
