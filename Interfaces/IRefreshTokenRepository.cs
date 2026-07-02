using BookStore.Presentation.Models;

namespace BookStore.Presentation.Interfaces;

public interface IRefreshTokenRepository
{
    RefreshToken? GetByHash(string hash);
    RefreshToken Create(RefreshToken refreshToken);
    bool DeleteByUserId(int userId);
}