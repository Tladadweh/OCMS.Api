using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCMS.Api.Auth;          
using OCMS.Application.DTO;   
using OCMS.Domain.Models;
using OCMS.Infrastructure.Persistence; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OCMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _hasher;
    private readonly ITokenService _tokens;

    public AuthController(AppDbContext db,
                          IPasswordHasher<User> hasher,
                          ITokenService tokens)
    {
        _db = db;
        _hasher = hasher;
        _tokens = tokens;
    }

    // POST: /api/auth/register-student
    [HttpPost("register-student")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto dto, CancellationToken ct)
    {
        bool exists = await _db.Users.AnyAsync(u =>
            u.UserName == dto.UserName || u.Email == dto.Email, ct);

        if (exists)
            return Conflict(new { message = "Username or Email already exists." });

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = dto.UserName.Trim(),
            Email = dto.Email.Trim(),
            Role = "Student"
        };

        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(RegisterStudent),
            new { id = user.Id },
            new { user.Id, user.UserName, user.Email, user.Role });
    }

    // POST: /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(
            u => u.UserName == dto.UserNameOrEmail || u.Email == dto.UserNameOrEmail, ct);

        if (user is null)
            return Unauthorized(new { message = "Invalid credentials." });

        var verification = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (verification == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Invalid credentials." });

        var token = _tokens.Generate(user);

        return Ok(new
        {
            access_token = token,
            token_type = "Bearer"
        });
    }

    // GET: /api/auth/me 
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Ok(new
        {
            id,
            name = User.Identity?.Name,
            role = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}
