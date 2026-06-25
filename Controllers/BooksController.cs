using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        var books = _booksService.GetAll();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = _booksService.GetById(id);
        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost]
    public ActionResult<Book> Create(Book book)
    {
        var createdBook = _booksService.Create(book);

        return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Book updatedBook)
    {
        var result = _booksService.Update(id, updatedBook);
        if (result is null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _booksService.Delete(id);
        if (result is false)
            return NotFound();

        return NoContent();
    }
}
