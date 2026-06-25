using BookStore.Presentation.Models;
using FluentValidation;

namespace BookStore.Presentation.Validations
{
    public sealed class BookValidator : AbstractValidator<Book>
    {
        public BookValidator() 
        { 
            RuleFor(book => book.PublishedDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Published date cannot be in the future.");
        }
    }
}
