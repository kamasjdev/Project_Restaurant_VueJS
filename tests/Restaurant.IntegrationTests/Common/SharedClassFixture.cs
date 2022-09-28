using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Common
{
    [CollectionDefinition("TestCollection")]
    public class SharedClassFixture : IClassFixture<TestApplicationFactory<Program>>, IAsyncDisposable
    {
        private static readonly SemaphoreSlim Semaphore = new(16); // 16 threads max in semaphore

        public async Task InitializeAsync() // Called before every test
        {
            await Semaphore.WaitAsync();
        }

        public ValueTask DisposeAsync()
        {
            Semaphore.Release();
            return ValueTask.CompletedTask;
        }
    }
}