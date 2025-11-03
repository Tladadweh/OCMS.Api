using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCMS.Application.DTO;
using OCMS.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OCMS.Api.Controllers
{
    [Route("api/courses/{courseId:guid}/lessons")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _svc;
        public LessonsController(ILessonService svc) => _svc = svc;

        // POST /api/courses/{courseId}/lessons
        [HttpPost]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Create([FromRoute] Guid courseId, [FromBody] LessonCreateDto dto, CancellationToken ct)
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(idStr, out var actorId)) return Unauthorized();

            try
            {
                var result = await _svc.CreateAsync(courseId, dto, actorId, role, ct);
                return CreatedAtAction(nameof(List), new { courseId }, result);
            }
            catch (KeyNotFoundException) { return NotFound(new { message = "Course not found." }); }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }

        // GET /api/courses/{courseId}/lessons
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> List([FromRoute] Guid courseId, CancellationToken ct)
        {
            var list = await _svc.ListAsync(courseId, ct);
            return Ok(list);
        }

        // PUT /api/courses/{courseId}/lessons/{lessonId}
        [HttpPut("{lessonId:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid courseId, [FromRoute] Guid lessonId,
            [FromBody] LessonUpdateDto dto, CancellationToken ct)
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            _ = Guid.TryParse(idStr, out var actorId);

            try
            {
                var updated = await _svc.UpdateAsync(courseId, lessonId, dto, actorId, role, ct);
                if (updated is null) return NotFound(new { message = "Lesson or Course not found." });
                return Ok(updated);
            }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }

        // DELETE /api/courses/{courseId}/lessons/{lessonId}
        [HttpDelete("{lessonId:guid}")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid courseId, [FromRoute] Guid lessonId, CancellationToken ct)
        {
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            _ = Guid.TryParse(idStr, out var actorId);

            try
            {
                var ok = await _svc.DeleteAsync(courseId, lessonId, actorId, role, ct);
                if (!ok) return NotFound(new { message = "Lesson or Course not found." });
                return NoContent();
            }
            catch (UnauthorizedAccessException) { return Forbid(); }
        }
    }
}
