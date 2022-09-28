using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Restaurant.IntegrationTests.Common
{
    public class TestApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        public HttpClient Client { get; }

        public TestApplicationFactory()
        {
            Client = Server.CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("database");
                DropDatabaseIfExists(connectionString);
            });
            builder.UseEnvironment("test");
        }

        private void DropDatabaseIfExists(string connectionString)
        {
            var fileName = GetDatabaseFileName(connectionString);
            SQLiteConnection.ClearAllPools();
            var filePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + fileName;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static string GetDatabaseFileName(string connectionString)
        {
            var connectionSplited = connectionString.Split(';').AsEnumerable();
            var dataSource = connectionSplited.Where(s => s.Contains("Data Source=")).FirstOrDefault();

            if (dataSource is null)
            {
                throw new InvalidOperationException("Invalid string, there is no 'Data Source='");
            }

            return dataSource.Split('=')[1];
        }
    }
}
