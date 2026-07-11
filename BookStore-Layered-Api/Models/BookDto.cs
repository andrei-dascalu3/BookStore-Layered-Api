namespace BookStore.Presentation.Models;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public DateOnly PublishedDate { get; set; }
}