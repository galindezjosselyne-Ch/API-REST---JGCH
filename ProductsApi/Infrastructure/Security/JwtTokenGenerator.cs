using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProductsApi.Infrastructure.Security;

public class JwtTokenGenerator
{
    private readonly string _secret;
    private readonly string _issuer;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _secret = configuration["JWT_SECRET"] ?? "DEV_SECRET_KEY_SUPER_LONG_2025xxxddd";
        _issuer = configuration["JWT_ISSUER"] ?? "ProductsApi";
    }

    public string GenerateToken(string username, string userId)
    {
        var key = Encoding.UTF8.GetBytes(_secret);

        if (key.Length < 16)
            throw new ArgumentException("JWT secret must be at least 16 bytes long.", nameof(_secret));

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Name, username)
            };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
