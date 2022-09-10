using Xunit;

namespace Restaurant.IntegrationTests.Common
{
    [CollectionDefinition("TestCollection")]
    public class SharedClassFixture : ICollectionFixture<TestApplicationFactory<Program>>
    {
    }
}