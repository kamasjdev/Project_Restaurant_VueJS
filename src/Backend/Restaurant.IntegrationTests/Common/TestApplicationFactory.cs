using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using System.Net.Http;
using System.Threading.Tasks;

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
            builder.UseEnvironment("test");
        }

        public async override ValueTask DisposeAsync()
        {
            await DeleteAllTables();
            await base.DisposeAsync();
        }

        private async Task DeleteAllTables()
        {
            var session = Services.GetRequiredService<ISession>();
            var query = session.CreateSQLQuery("select name from sqlite_master where type is 'table'");
            var tables = query.List<string>();
            
            foreach(var table in tables)
            {
                var queryDelete = session.CreateSQLQuery($"drop table if exists {table}");
                await queryDelete.ExecuteUpdateAsync();
            }
        }
    }
}
