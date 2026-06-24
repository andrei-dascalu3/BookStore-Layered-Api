using BookStore.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    internal static readonly List<Author> Authors =
    [
        new Author { Id = 1, FirstName = "George", LastName = "Orwell" },
        new Author { Id = 2, FirstName = "Jane", LastName = "Austen" },
        new Author { Id = 3, FirstName = "Mark", LastName = "Twain" }
    ];

    private static int _nextId = 4;

    [HttpGet]
    public ActionResult<IEnumerable<Author>> GetAll()
    {
        return Ok(Authors);
    }

    [HttpGet("{id}")]
    public ActionResult<Author> GetById(int id)
    {
        var author = Authors.FirstOrDefault(a => a.Id == id);
        if (author is null)
            return NotFound();

        return Ok(author);
    }

    [HttpGet("{id}/books")]
    public ActionResult<IEnumerable<Book>> GetBooksByAuthor(int id)
    {
        var author = Authors.FirstOrDefault(a => a.Id == id);
        if (author is null)
            return NotFound();

        var books = BooksController.Books.Where(b => b.AuthorId == id).ToList();

        return Ok(books);
    }

    [HttpPost]
    public ActionResult<Author> Create(Author author)
    {
        author.Id = _nextId++;
        Authors.Add(author);

        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Author updatedAuthor)
    {
        var author = Authors.FirstOrDefault(a => a.Id == id);
        if (author is null)
            return NotFound();

        author.FirstName = updatedAuthor.FirstName;
        author.LastName = updatedAuthor.LastName;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var author = Authors.FirstOrDefault(a => a.Id == id);
        if (author is null)
            return NotFound();

        Authors.Remove(author);

        return NoContent();
    }
}
