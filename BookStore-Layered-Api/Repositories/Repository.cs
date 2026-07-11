using BookStore.Presentation.Database;
using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Presentation.Repositories;

internal sealed class Repository : IRepository
{
    private readonly BookStoreDbContext _context;

    public Repository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books.AsNoTracking().ToListAsync();
    }

    public Book? GetById(int id)
    {
        return _context.Books.Find(id);
    }

    public Book Create(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
        return book;
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
        _context.SaveChanges();
    }

    public bool Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book is null) return false;

        _context.Books.Remove(book);
        _context.SaveChanges();
        return true;
    }
}