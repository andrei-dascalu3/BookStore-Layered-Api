using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public ActionResult<AuthResponse> Register(RegisterRequest request)
    {
        var result = _authService.Register(request);
        if (result == null)
        {
            return Conflict("Username already exists.");
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(LoginRequest request)
    {
        var result = _authService.Login(request);
        if (result == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(result);
    }

    [HttpPost("refresh")]
    public ActionResult<AuthResponse> Refresh(RefreshRequest request)
    {
        var result = _authService.Refresh(request);
        if (result is null)
            return Unauthorized("Invalid or expired refresh token.");

        return Ok(result);
    }
}
