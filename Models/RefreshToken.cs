namespace BookStore.Presentation.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Hash { get; set; } = string.Empty;
    public DateTime TokenExpiry { get; set; }
    public int UserId { get; set; }
}
