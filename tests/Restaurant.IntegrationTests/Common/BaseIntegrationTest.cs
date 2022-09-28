using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.DTO;
using Shouldly;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Common
{
    [Collection("TestCollection")]
    public class BaseIntegrationTest : IAsyncLifetime
    {
        protected async Task Authorize(IFlurlClient client)
        {
            var dto = new SignInDto("admin@admin.com", "PasW0Rd!26");
            var response = await client.Request("/api/users/sign-in").PostJsonAsync(dto);
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var auth = await response.ResponseMessage.Content.ReadFromJsonAsync<AuthDto>();
            client.WithOAuthBearerToken(auth.AccessToken);
        }

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

        public async Task InitializeAsync()
        {
            await Authorize(_client);
        }

        public Task DisposeAsync()
            => Task.CompletedTask;

        protected T GetRequiredService<T>()
        {
            return _factory.Services.GetRequiredService<T>();
        }

        protected T GetService<T>()
        {
            return _factory.Services.GetService<T>();
        }

        protected readonly IFlurlClient _client;
        private readonly TestApplicationFactory<Program> _factory;

        public BaseIntegrationTest(TestApplicationFactory<Program> factory)
        {
            _client = new FlurlClient(factory.Client);
            _factory = factory;
        }
    }
}
