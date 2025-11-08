using ProductsApi.Application.Interfaces;
using ProductsApi.Infrastructure.Security;

namespace ProductsApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly JwtTokenGenerator _jwt;

    public AuthService(JwtTokenGenerator jwt)
    {
        _jwt = jwt;
    }

    public string? Authenticate(string username, string password)
    {
        if (username == "test@local" && password == "Password123")
            return _jwt.GenerateToken(username, "1");
        return null;
    }
}
