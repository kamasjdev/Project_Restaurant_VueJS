using FluentMigrator.Runner;
using Microsoft.Extensions.Hosting;

namespace Restaurant.Infrastructure.Initializers
{
    internal sealed class DbInitializer : IHostedService
    {
        private readonly IMigrationRunner _migrationRunner;

        public DbInitializer(IMigrationRunner migrationRunner)
        {
            _migrationRunner = migrationRunner;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            _migrationRunner.MigrateUp();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
