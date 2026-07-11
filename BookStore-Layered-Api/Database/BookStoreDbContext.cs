using BookStore.Presentation.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BookStore.Presentation.Database;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().Property(b => b.Price).HasPrecision(18, 2);
    }
}