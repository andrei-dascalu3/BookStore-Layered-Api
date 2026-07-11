using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
    {
        var books = await _booksService.GetAllAsync();
        return Ok(books);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public ActionResult<BookDto> GetById(int id)
    {
        var book = _booksService.GetById(id);
        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost]
    public ActionResult<BookDto> Create(Book book)
    {
        var createdBook = _booksService.Create(book);

        if (createdBook is null)
            return BadRequest("Invalid book data.");

        return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("{id}")]
    public ActionResult<BookDto> Update(int id, Book updatedBook)
    {
        var result = _booksService.Update(id, updatedBook);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _booksService.Delete(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
