using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCMS.Application.DTO;
using OCMS.Domain.Models;
using OCMS.Infrastructure.Persistence;

namespace OCMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        public UsersController(AppDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
        }


        // POST: /api/users  
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken ct)
        {
            bool exists = await _db.Users.AnyAsync(u =>
                u.UserName == dto.UserName || u.Email == dto.Email, ct);
            if (exists) return Conflict(new { message = "Username or Email already exists." });

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName.Trim(),
                Email = dto.Email.Trim(),
                Role = dto.Role.Trim()
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(Create), new { id = user.Id },
                new { user.Id, user.UserName, user.Email, user.Role });
        }

        // PUT: /api/users/{id}/role 
        [HttpPut("{id:guid}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole([FromRoute] Guid id, [FromBody] UpdateUserRoleDto dto, CancellationToken ct)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (user is null) return NotFound(new { message = "User not found." });

            user.Role = dto.Role.Trim();
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);

            return Ok(new { user.Id, user.UserName, user.Email, user.Role });
        }


        // PUT: /api/users/me/profile 
        [HttpPut("me/profile")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMyProfileDto dto, CancellationToken ct)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrWhiteSpace(idStr) || !Guid.TryParse(idStr, out var myId))
                return Unauthorized(new { message = "Invalid token." });

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == myId, ct);
            if (user is null) return NotFound(new { message = "User not found." });

            if (dto.Email is not null)
            {
                var email = dto.Email.Trim();
                bool taken = await _db.Users.AnyAsync(u => u.Email == email && u.Id != myId, ct);
                if (taken) return Conflict(new { message = "Email already in use." });
                user.Email = email;
            }

            if (dto.FullName is not null) user.FullName = dto.FullName.Trim();
            if (dto.Bio is not null) user.Bio = dto.Bio.Trim();
            if (dto.PhotoUrl is not null) user.PhotoUrl = dto.PhotoUrl.Trim();

            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.FullName,
                user.Email,
                user.Bio,
                user.PhotoUrl,
                user.Role
            });
        }
    }
}
