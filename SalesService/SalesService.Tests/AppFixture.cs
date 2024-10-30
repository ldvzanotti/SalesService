using Alba;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using SalesService.Api;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Orders.Events;
using SalesService.Persistence;
using System.Reflection;
using System.Text;

namespace SalesService.Tests
{
    public class AppFixture : IDisposable
    {
        public IAlbaHost Host;
        public string Jwt = _configuration.GetValue<string>("TestJwt");

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
                        .SetBasePath(Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).Location))
                        .AddJsonFile("appsettings.Development.json", optional: false)
                        .Build();

        private string SchemaName { get; } = "testschema" + Guid.NewGuid().ToString().Replace("-", string.Empty);

        private bool _disposed = false;

        public AppFixture()
        {
            Host = AlbaHost.For<Program>(config =>
            {
                config.ConfigureServices(services =>
                {
                    services.AddNpgsqlDataSource(_configuration.GetConnectionString("DefaultConnection"));

                    services.AddMarten(options =>
                    {
                        options.UseSystemTextJsonForSerialization();
                        options.DatabaseSchemaName = SchemaName;
                        options.Projections.Add<OrderProjection>(ProjectionLifecycle.Inline);

                        options.ApplicationAssembly = typeof(OrderCreated).Assembly;
                    })
                    .InitializeWith(new SeedData())
                    .UseNpgsqlDataSource()
                    .AddAsyncDaemon(DaemonMode.Solo);

                    services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = "http://localhost:8080",
                    ValidateIssuer = true,
                    ValidIssuer = "dotnet-user-jwts",
                    RequireExpirationTime = false,
                    ValidateIssuerSigningKey = false
                };
                    });
                });
            }).GetAwaiter().GetResult();

            Host.Start();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Host.CleanAllMartenDataAsync().Wait();
                    Host.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
