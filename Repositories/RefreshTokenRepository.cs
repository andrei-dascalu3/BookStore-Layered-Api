using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;

namespace BookStore.Presentation.Repositories;

// Improperly named, the DB table only stores the hash value of the refresh token
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private static readonly IList<RefreshToken> _refreshTokens = new List<RefreshToken>();

    private static int _nextId = 1;

    public RefreshToken? GetByHash(string hash)
    {
        return _refreshTokens.FirstOrDefault(rt => rt.Hash == hash);
    }

    public RefreshToken Create(RefreshToken refreshToken)
    {
        refreshToken.Id = _nextId++;
        _refreshTokens.Add(refreshToken);

        return refreshToken;
    }

    public bool DeleteByUserId(int userId)
    {
        var refreshToken = _refreshTokens.FirstOrDefault(r => r.UserId == userId);
        if (refreshToken == null)
        {
            return false;
        }
        _refreshTokens.Remove(refreshToken);

        return true;
    }
}
