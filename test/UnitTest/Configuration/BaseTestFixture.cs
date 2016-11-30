using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAPIApplication;

namespace UnitTest
{
    public class BaseTestFixture : IDisposable
    {
        public readonly TestServer Server;
        public readonly HttpClient Client;
        public readonly DataContext TestDataContext;
        public readonly IConfigurationRoot Configuration;

        public BaseTestFixture()
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var opts = new DbContextOptionsBuilder<DataContext>();
            opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            TestDataContext = new DataContext(opts.Options);
            SetupDatabase();

            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        private void SetupDatabase()
        {
            try
            {
                TestDataContext.Database.EnsureCreated();
                TestDataContext.Database.Migrate();
            }
            catch (Exception)
            {
                //TODO: Add a better logging
                // Does nothing
            }
        }

        public void Dispose()
        {
            TestDataContext.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}