using BookStore.Presentation.Models;

namespace BookStore.Presentation.Interfaces;

public interface IUserRepository
{
    User? GetByUsername(string username);

    User Add(User user);

    User? GetById(int id);
}
