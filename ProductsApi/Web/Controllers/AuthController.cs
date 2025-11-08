using Microsoft.AspNetCore.Mvc;
using ProductsApi.Application.Interfaces;

namespace ProductsApi.Web.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        var token = _auth.Authenticate(req.Username, req.Password);
        if (token == null) return Unauthorized();
        return Ok(new { token });
    }
}

public record LoginRequest(string Username, string Password);
