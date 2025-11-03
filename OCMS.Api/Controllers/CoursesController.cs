using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCMS.Application.Services;
using OCMS.Application.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static OCMS.Application.DTO.CourseDtos;

namespace OCMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _svc;

        public CoursesController(ICourseService svc) => _svc = svc;

        // POST: /api/courses
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto dto, CancellationToken ct)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(idStr, out var instructorId))
                return Unauthorized(new { message = "Invalid token." });

            try
            {
                var result = await _svc.CreateAsync(dto, instructorId, ct);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("code", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { message = "Course code already exists." });
            }
        }

        // GET: /api/courses/{id}
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var dto = await _svc.GetByIdAsync(id, ct);
            return dto is null ? NotFound(new { message = "Course not found." }) : Ok(dto);
        }

        // PUT: /api/courses/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CourseUpdateDto dto, CancellationToken ct)
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            _ = Guid.TryParse(idStr, out var actorId);

            try
            {
                var updated = await _svc.UpdateAsync(id, dto, actorId, role, ct);
                if (updated is null) return NotFound(new { message = "Course not found." });
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

        }

        // DELETE: /api/courses/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            _ = Guid.TryParse(idStr, out var actorId);

            try
            {
                var ok = await _svc.DeleteAsync(id, actorId, role, ct);
                if (!ok) return NotFound(new { message = "Course not found." });
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // POST: /api/courses/{id}/publish
        [HttpPost("{id:guid}/publish")]
        [Authorize(Roles = "Instructor")] 
        public async Task<IActionResult> Publish([FromRoute] Guid id, CancellationToken ct)
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(idStr, out var actorId)) return Unauthorized();

            try
            {
                var dto = await _svc.PublishAsync(id, actorId, role, ct);
                return Ok(dto); 
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Course not found." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); 
            }
        }

        // POST: /api/courses/{id}/enroll
        [HttpPost("{id:guid}/enroll")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Enroll([FromRoute] Guid id, [FromServices] IEnrollmentService svc, CancellationToken ct)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(idStr, out var studentId)) return Unauthorized();

            try
            {
                await svc.EnrollAsync(id, studentId, ct);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Course not found." });
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(new { message = ex.Message }); 
            }
        }

        // GET: /api/courses/{id}/my-progress
        [HttpGet("{id:guid}/my-progress")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyProgress([FromRoute] Guid id, [FromServices] IProgressService svc, CancellationToken ct)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(idStr, out var studentId)) return Unauthorized();

            try
            {
                var dto = await svc.GetMyCourseProgressAsync(id, studentId, ct);
                return Ok(dto); 
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // GET: /api/courses/search
        [HttpGet("search")]
        [AllowAnonymous] 
        public async Task<IActionResult> Search(
    [FromQuery] string? keyword,
    [FromQuery] string? category,
    [FromQuery] Guid? instructorId,
    [FromServices] ICourseQueryService svc,
    CancellationToken ct)
        {
            string? role = null;
            if (User?.Identity?.IsAuthenticated == true)
            {
                role = User.FindFirstValue(ClaimTypes.Role)
                       ?? User.FindFirstValue("role");
            }

            var results = await svc.SearchAsync(keyword, category, instructorId, role, ct);
            return Ok(results);
        }
        // GET: /api/courses/top
        [HttpGet("top")]
        [AllowAnonymous]
        public async Task<IActionResult> Top([FromServices] ICourseQueryService svc, [FromQuery] TopQuery q,CancellationToken ct = default)
        {
            var items = await svc.TopAsync(q.Take, ct);
            return Ok(items);
        }

        // GET: /api/courses/{id}/roster
        [HttpGet("{id:guid}/roster")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Roster([FromRoute] Guid id, [FromServices] IRosterService svc, CancellationToken ct)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "Student";
            if (!Guid.TryParse(idStr, out var actorId)) return Unauthorized();

            try
            {
                var items = await svc.GetRosterAsync(id, actorId, role, ct);
                return Ok(items);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Course not found." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }



    }

}
