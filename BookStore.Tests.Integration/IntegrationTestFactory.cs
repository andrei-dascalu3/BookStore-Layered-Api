using System.Data.Common;
using BookStore.Presentation.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using Respawn;
using BookStore.Presentation;

namespace BookStore.IntegrationTests;

public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public string ConnectionString { get; set; } = "Server=(localdb)\\MSSQLLocalDB;Database=BookStoreTest;Trusted_Connection=True;TrustServerCertificate=True";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookStoreDbContext>));
            if (descriptor != null) services.Remove(descriptor);

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
            SchemasToInclude = new[] { "dbo" }
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