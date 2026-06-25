using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;

namespace BookStore.Presentation.Services
{
    internal sealed class BooksService : IBooksService
    {
        private readonly IRepository _repository;
        
        public BooksService(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Book> GetAll()
        {
            var books = _repository.GetAll();
            return books;
        }

        public Book? GetById(int id)
        {
            var book = _repository.GetById(id);
            return book;
        }

        public Book Create(Book book)
        {
            var createdBook = _repository.Create(book);

            return createdBook;
        }

        public Book? Update(int id, Book updatedBook)
        {
            var book = _repository.GetById(id);
            if (book is null)
                return null;

            book.Title = updatedBook.Title;
            book.AuthorName = updatedBook.AuthorName;
            book.Genre = updatedBook.Genre;
            book.Price = updatedBook.Price;
            book.PublishedDate = updatedBook.PublishedDate;

            _repository.Update(book);

            return book;
        }

        public bool Delete(int id)
        {
            var isDeleted = _repository.Delete(id);

            return isDeleted;
        }
    }
}
