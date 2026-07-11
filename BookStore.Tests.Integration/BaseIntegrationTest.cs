using Microsoft.Extensions.DependencyInjection;
using BookStore.Presentation.Database;

namespace BookStore.IntegrationTests;

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