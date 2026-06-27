using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using FluentValidation;
using Mapster;

namespace BookStore.Presentation.Services
{
    internal sealed class BooksService : IBooksService
    {
        private readonly IRepository _repository;
        private readonly IValidator<Book> _validator;

        public BooksService(IRepository repository, IValidator<Book> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _repository.GetAllAsync();
            var bookDto = books.Adapt<List<BookDto>>();

            return bookDto;
        }

        public BookDto? GetById(int id)
        {
            var book = _repository.GetById(id);
            var bookDto = book.Adapt<BookDto>();

            return bookDto;
        }

        public BookDto? Create(Book book)
        {
            var validationResult = _validator.Validate(book);
            if (validationResult.IsValid)
            {
                var createdBook = _repository.Create(book);
                var createdBookDto = createdBook.Adapt<BookDto>();
                return createdBookDto;
            }

            return null;
        }

        public BookDto? Update(int id, Book updatedBook)
        {
            var validationResult = _validator.Validate(updatedBook);
            if (!validationResult.IsValid)
                return null;
            
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
