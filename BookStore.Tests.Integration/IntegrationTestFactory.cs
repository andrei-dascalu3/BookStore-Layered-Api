using BookStore.Presentation;
using BookStore.Presentation.Database;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using System.Data.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BookStore.Tests.Integration;

public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public string ConnectionString { get; set; } = "Server=(localdb)\\MSSQLLocalDB;Database=BookStoreTest;Trusted_Connection=True;TrustServerCertificate=True";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookStoreDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });
        });
    }

    public async Task InitializeAsync()
    {
        _dbConnection = new SqlConnection(ConnectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["dbo"]
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public new async Task DisposeAsync()
    {
        await _dbConnection.CloseAsync();
    }
}

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFactory> { }