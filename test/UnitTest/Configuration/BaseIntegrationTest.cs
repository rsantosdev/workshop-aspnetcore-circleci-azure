using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using WebAPIApplication;
using Xunit;

namespace UnitTest
{
    [Collection("Base collection")]
    public abstract class BaseIntegrationTest
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;
        protected readonly DataContext TestDataContext;

        protected BaseTestFixture Fixture { get; }

        protected BaseIntegrationTest(BaseTestFixture fixture)
        {
          Fixture = fixture;

          TestDataContext = fixture.TestDataContext;
          Server = fixture.Server;
          Client = fixture.Client;

          ClearDb().Wait();
        }

        private async Task ClearDb()
        {
            var commands = new[]
            {
                "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'",
                "EXEC sp_MSForEachTable 'DELETE FROM ?'",
                "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'"
            };

            await TestDataContext.Database.OpenConnectionAsync();

            foreach (var command in commands)
            {
                await TestDataContext.Database.ExecuteSqlCommandAsync(command);
            }

            TestDataContext.Database.CloseConnection();
        }
    }
}