using BookStore.Presentation.Models;
using BookStore.Presentation.Validations;
using FluentValidation.TestHelper;
using Shouldly;

namespace BookStore.UnitTests.Validators
{
    public class BookValidatorTests
    {
        private readonly BookValidator _validator = new();

        [Fact]
        public void When_PublishedDateIsToday_Expect_NotHaveValidationError()
        {
            var book = new Book
            {
                PublishedDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = _validator.TestValidate(book);

            result.ShouldNotHaveValidationErrorFor(b => b.PublishedDate);
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData(-30)]
        [InlineData(-1)]
        [InlineData(-365)]
        [InlineData(-3650)]
        public void When_PublishedDateIsInThePast_Expect_NotHaveValidationError(int daysOffset)
        {
            var book = new Book
            {
                PublishedDate = DateOnly.FromDateTime(DateTime.Now).AddDays(daysOffset)
            };

            var result = _validator.TestValidate(book);

            result.ShouldNotHaveValidationErrorFor(b => b.PublishedDate);
            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void When_PublishedDateIsInTheFuture_Expect_HaveValidationError()
        {
            var book = new Book
            {
                PublishedDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1)
            };

            var result = _validator.TestValidate(book);

            result.ShouldHaveValidationErrorFor(b => b.PublishedDate)
                .WithErrorMessage("Published date cannot be in the future.");
            result.IsValid.ShouldBeFalse();
        }
    }
}
