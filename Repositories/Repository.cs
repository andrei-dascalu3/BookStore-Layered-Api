using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;

namespace BookStore.Presentation.Repositories
{
    internal sealed class Repository : IRepository
    {
        private static readonly List<Book> Books =
        [
            new Book { Id = 1, Title = "1984", AuthorName = "George Orwell", Genre = "Dystopian", Price = 9.99m, PublishedDate = new DateOnly(1949, 6, 8) },
            new Book { Id = 2, Title = "Animal Farm", AuthorName = "George Orwell", Genre = "Satire", Price = 7.99m, PublishedDate = new DateOnly(1945, 8, 17) },
            new Book { Id = 3, Title = "Pride and Prejudice", AuthorName = "Jane Austen", Genre = "Romance", Price = 8.49m, PublishedDate = new DateOnly(1813, 1, 28) },
            new Book { Id = 4, Title = "Adventures of Huckleberry Finn", AuthorName = "Mark Twain", Genre = "Adventure", Price = 6.99m, PublishedDate = new DateOnly(1884, 12, 10) }
        ];

        private static int _nextId = 5;

        public List<Book> GetAll()
        {
            return Books;
        }

        public Book? GetById(int id)
        {
            return Books.FirstOrDefault(b => b.Id == id);
        }

        public Book Create(Book book)
        {
            book.Id = _nextId++;
            Books.Add(book);

            return book;
        }

        public void Update(Book book)
        {
            // The object is modified through reference, so no need to do anything here.
        }

        public bool Delete(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book is null)
                return false;

            Books.Remove(book);

            return true;
        }
    }
}
