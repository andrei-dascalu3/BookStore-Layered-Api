using BookStore.Presentation.Models;

namespace BookStore.Presentation.Interfaces
{
    public interface IBooksService
    {
        IEnumerable<BookDto> GetAll();
        BookDto? GetById(int id);
        BookDto? Create(Book book);
        BookDto? Update(int id, Book updatedBook);
        bool Delete(int id);
    }
}
