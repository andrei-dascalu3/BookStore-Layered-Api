using BookStore.Presentation.Models;

namespace BookStore.Presentation.Interfaces;

public interface IAuthService
{
    AuthResponse? Register(RegisterRequest request);
    AuthResponse? Login(LoginRequest request);
    AuthResponse? Refresh(RefreshRequest request);
    bool Logout(string username);
}
