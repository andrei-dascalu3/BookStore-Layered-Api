using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using BookStore.Presentation.Services;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Shouldly;

namespace BookStore.UnitTests.Services
{
    public class BooksServiceeTests
    {
        private readonly Mock<IRepository> _repositoryMock = new();
        private readonly Mock<IValidator<Book>> _validatorMock = new();
        private readonly BooksService _sut;

        public BooksServiceeTests()
        {
            _sut = new BooksService(_repositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_Expect_ReturnAllBooksAsDto()
        {
            var books = new List<Book>
            {
                new() { Id = 1, Title = "Book 1" },
                new() { Id = 2, Title = "Book 2" }
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

            var result = await _sut.GetAllAsync();

            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            result.Select(b => b.Title).ShouldBe(new[] { "Book 1", "Book 2" });
        }

        [Fact]
        public void GetById_When_BookExists_Expect_ReturnBookDto()
        {
            var book = new Book { Id = 1, Title = "Book 1" };
            _repositoryMock.Setup(r => r.GetById(1)).Returns(book);

            var result = _sut.GetById(1);

            result.ShouldNotBeNull();
            result!.Id.ShouldBe(1);
            result.Title.ShouldBe("Book 1");
        }

        [Fact]
        public void GetById_When_BookDoesNotExist_Expect_ReturnNull()
        {
            _repositoryMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Book?)null);

            var result = _sut.GetById(1);

            result.ShouldBeNull();
        }

        [Fact]
        public void Create_When_ValidationSucceeds_Expect_CreateBookAndReturnDto()
        {
            var book = new Book { Id = 1, Title = "New Book" };
            _validatorMock.Setup(v => v.Validate(book)).Returns(new ValidationResult());
            _repositoryMock.Setup(r => r.Create(book)).Returns(book);

            var result = _sut.Create(book);

            result.ShouldNotBeNull();
            result!.Title.ShouldBe("New Book");
            _repositoryMock.Verify(r => r.Create(book), Times.Once);
        }

        [Fact]
        public void Create_When_ValidationFails_Expect_ReturnNullAndNotCreate()
        {
            var book = new Book { Id = 1, Title = "New Book" };
            var failures = new List<ValidationFailure> { new("PublishedDate", "Invalid date") };
            _validatorMock.Setup(v => v.Validate(book)).Returns(new ValidationResult(failures));

            var result = _sut.Create(book);

            result.ShouldBeNull();
            _repositoryMock.Verify(r => r.Create(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public void Update_When_ValidationFails_Expect_ReturnNullAndNotUpdate()
        {
            var updatedBook = new Book { Title = "Updated" };
            var failures = new List<ValidationFailure> { new("PublishedDate", "Invalid date") };
            _validatorMock.Setup(v => v.Validate(updatedBook)).Returns(new ValidationResult(failures));

            var result = _sut.Update(1, updatedBook);

            result.ShouldBeNull();
            _repositoryMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public void Update_When_ValidationSucceedsButBookNotFound_Expect_ReturnNull()
        {
            var updatedBook = new Book { Title = "Updated" };
            _validatorMock.Setup(v => v.Validate(updatedBook)).Returns(new ValidationResult());
            _repositoryMock.Setup(r => r.GetById(1)).Returns((Book?)null);

            var result = _sut.Update(1, updatedBook);

            result.ShouldBeNull();
            _repositoryMock.Verify(r => r.Update(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public void Update_When_ValidationSucceedsAndBookExists_Expect_UpdateBookAndReturnDto()
        {
            var existingBook = new Book { Id = 1, Title = "Old", AuthorName = "Old Author", Genre = "Old Genre", Price = 10, PublishedDate = DateOnly.FromDateTime(DateTime.Now) };
            var updatedBook = new Book { Title = "New", AuthorName = "New Author", Genre = "New Genre", Price = 20, PublishedDate = DateOnly.FromDateTime(DateTime.Now) };
            _validatorMock.Setup(v => v.Validate(updatedBook)).Returns(new ValidationResult());
            _repositoryMock.Setup(r => r.GetById(1)).Returns(existingBook);

            var result = _sut.Update(1, updatedBook);

            result.ShouldNotBeNull();
            result!.Title.ShouldBe("New");
            result.AuthorName.ShouldBe("New Author");
            result.Genre.ShouldBe("New Genre");
            _repositoryMock.Verify(r => r.Update(It.Is<Book>(b => b.Title == "New")), Times.Once);
        }

        [Fact]
        public void Delete_When_BookExists_Expect_ReturnTrue()
        {
            _repositoryMock.Setup(r => r.Delete(1)).Returns(true);

            var result = _sut.Delete(1);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Delete_When_BookDoesNotExist_Expect_ReturnFalse()
        {
            _repositoryMock.Setup(r => r.Delete(1)).Returns(false);

            var result = _sut.Delete(1);

            result.ShouldBeFalse();
        }
    }
}
