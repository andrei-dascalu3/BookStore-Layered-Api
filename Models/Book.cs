namespace BookStore.Presentation.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public string Genre { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly PublishedDate { get; set; }
}
