using System.Net;
using System.Net.Http.Json;
using BookStore.Presentation.Models;
using FluentAssertions;

namespace BookStore.IntegrationTests;

public class BooksControllerTests : BaseIntegrationTest
{
    public BooksControllerTests(IntegrationTestFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAll_ShouldReturnAllBooksFromDatabase()
    {
        var testBook = new Book { Title = "Test Book", AuthorName = "Author", Genre = "SciFi", Price = 12.99m, PublishedDate = new DateOnly(2020, 1, 1) };
        DbContext.Books.Add(testBook);
        await DbContext.SaveChangesAsync();
        DbContext.Entry(testBook).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

        // Act
        var response = await HttpClient.GetAsync("api/books");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<BookDto>>();
        result.Should().NotBeNull().And.HaveCount(1);
        result![0].Title.Should().Be("Test Book");
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await HttpClient.GetAsync("api/books/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}