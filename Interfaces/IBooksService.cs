using BookStore.Presentation.Models;

namespace BookStore.Presentation.Interfaces
{
    public interface IBooksService
    {
        IEnumerable<Book> GetAll();
        Book? GetById(int id);
        Book Create(Book book);
        Book? Update(int id, Book updatedBook);
        bool Delete(int id);
    }
}
