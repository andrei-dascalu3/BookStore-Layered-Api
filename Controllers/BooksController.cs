using BookStore.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    internal static readonly List<Book> Books =
    [
        new Book { Id = 1, Title = "1984", AuthorId = 1, Genre = "Dystopian", Price = 9.99m, PublishedDate = new DateOnly(1949, 6, 8) },
        new Book { Id = 2, Title = "Animal Farm", AuthorId = 1, Genre = "Satire", Price = 7.99m, PublishedDate = new DateOnly(1945, 8, 17) },
        new Book { Id = 3, Title = "Pride and Prejudice", AuthorId = 2, Genre = "Romance", Price = 8.49m, PublishedDate = new DateOnly(1813, 1, 28) },
        new Book { Id = 4, Title = "Adventures of Huckleberry Finn", AuthorId = 3, Genre = "Adventure", Price = 6.99m, PublishedDate = new DateOnly(1884, 12, 10) }
    ];

    private static int _nextId = 5;

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(Books);
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost]
    public ActionResult<Book> Create(Book book)
    {
        if (!AuthorsController.Authors.Any(a => a.Id == book.AuthorId))
            return BadRequest($"Author with id {book.AuthorId} does not exist.");

        book.Id = _nextId++;
        Books.Add(book);

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Book updatedBook)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();

        if (!AuthorsController.Authors.Any(a => a.Id == updatedBook.AuthorId))
            return BadRequest($"Author with id {updatedBook.AuthorId} does not exist.");

        book.Title = updatedBook.Title;
        book.AuthorId = updatedBook.AuthorId;
        book.Genre = updatedBook.Genre;
        book.Price = updatedBook.Price;
        book.PublishedDate = updatedBook.PublishedDate;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return NotFound();

        Books.Remove(book);

        return NoContent();
    }
}
