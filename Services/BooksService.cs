using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using Mapster;

namespace BookStore.Presentation.Services
{
    internal sealed class BooksService : IBooksService
    {
        private readonly IRepository _repository;
        
        public BooksService(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<BookDto> GetAll()
        {
            var books = _repository.GetAll();
            var bookDto = books.Adapt<List<BookDto>>();

            return bookDto;
        }

        public BookDto? GetById(int id)
        {
            var book = _repository.GetById(id);
            var bookDto = book.Adapt<BookDto>();

            return bookDto;
        }

        public BookDto Create(Book book)
        {
            var createdBook = _repository.Create(book);
            var createdBookDto = createdBook.Adapt<BookDto>();

            return createdBookDto;
        }

        public BookDto? Update(int id, Book updatedBook)
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

            var bookDto = book.Adapt<BookDto>();

            return bookDto;
        }

        public bool Delete(int id)
        {
            var isDeleted = _repository.Delete(id);

            return isDeleted;
        }
    }
}
