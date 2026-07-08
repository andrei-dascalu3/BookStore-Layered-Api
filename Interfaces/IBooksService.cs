using BookStore.Presentation.Models;

namespace BookStore.Presentation.Interfaces
{
    public interface IBooksService
    {
        Task<IQueryable<BookDto>> GetAllAsync();
        BookDto? GetById(int id);
        BookDto? Create(Book book);
        BookDto? Update(int id, Book updatedBook);
        bool Delete(int id);
    }
}
