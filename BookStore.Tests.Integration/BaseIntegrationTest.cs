using BookStore.Presentation.Database;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Tests.Integration;

[Collection("IntegrationTests")]
public abstract class BaseIntegrationTest : IAsyncLifetime
{
    protected readonly IntegrationTestFactory Factory;
    protected readonly HttpClient HttpClient;
    protected readonly IServiceScope Scope;
    protected readonly BookStoreDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestFactory factory)
    {
        Factory = factory;
        HttpClient = factory.CreateClient();
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
    }

    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
        Scope.Dispose();
    }
}