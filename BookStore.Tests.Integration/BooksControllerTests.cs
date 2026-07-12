using System.Net;
using System.Net.Http.Json;
using BookStore.Presentation.Models;
using FluentAssertions;

namespace BookStore.Tests.Integration;


public class BooksControllerTests(IntegrationTestFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetAll_ShouldReturnAllBooksFromDatabase()
    {
        // TODO: implement first integration test
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        /**
         * Exercise 1
         *
         * Verify that making a GET request to api/books/999 returns an HTTP 404 NotFound status code
         */
    }

    [Fact]
    public async Task Create_WithFuturePublishedDate_ShouldReturnBadRequest()
    {
        /**
         * Exercise 2
         *
         * Send a POST request to api/books and check returned response
         * Verify that the database is still completely empty
         */
    }
}