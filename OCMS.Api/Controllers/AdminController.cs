using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCMS.Application.DTO;
using OCMS.Domain.Models;
using OCMS.Infrastructure.Persistence;

namespace OCMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] 
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        public AdminController(AppDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        // POST /api/users  
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken ct)
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
                Role = dto.Role.Trim()
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(Create), new { id = user.Id },
                new { user.Id, user.UserName, user.Email, user.Role });
        }

        // PUT /api/users/{id}/role 
        [HttpPut("{id:guid}/role")]
        public async Task<IActionResult> UpdateRole([FromRoute] Guid id, [FromBody] UpdateUserRoleDto dto, CancellationToken ct)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (user is null) return NotFound(new { message = "User not found." });

            user.Role = dto.Role.Trim();
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);

            return Ok(new { user.Id, user.UserName, user.Email, user.Role });
        }
    }
}
