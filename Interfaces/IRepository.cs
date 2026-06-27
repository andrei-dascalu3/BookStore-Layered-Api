using BookStore.Presentation.Models;

namespace BookStore.Presentation.Interfaces
{
    public interface IRepository
    {
        Task<List<Book>> GetAllAsync();
        Book? GetById(int id);
        Book Create(Book book);
        void Update(Book book);
        bool Delete(int id);
    }
}
