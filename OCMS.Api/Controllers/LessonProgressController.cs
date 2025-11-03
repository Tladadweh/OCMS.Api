using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCMS.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OCMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]

    public class LessonProgressController : ControllerBase
    {
        private readonly IProgressService _svc;
        public LessonProgressController(IProgressService svc) => _svc = svc;

        // POST: /api/lessons/{lessonId}/complete
        [HttpPost("{lessonId:guid}/complete")]
        public async Task<IActionResult> Complete([FromRoute] Guid lessonId, CancellationToken ct)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(idStr, out var studentId)) return Unauthorized();

            try
            {
                var result = await _svc.CompleteLessonAsync(lessonId, studentId, ct);
                return Ok(result); 
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Lesson not found." });
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

    }
}
