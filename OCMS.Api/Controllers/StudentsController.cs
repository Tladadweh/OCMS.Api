using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCMS.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OCMS.Api.Controllers
{
    [ApiController]
    [Route("api/students/me")]
    [Authorize(Roles = "Student")]
    public class StudentsController : ControllerBase
    {
        private readonly IEnrollmentService _svc;
        private readonly IProgressService _progressSvc;

        public StudentsController(IEnrollmentService svc, IProgressService progressSvc)
        {
            _svc = svc;
            _progressSvc = progressSvc;
        }

        // GET: /api/students/me/enrollments
        [HttpGet("enrollments")]
        public async Task<IActionResult> MyEnrollments(CancellationToken ct)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(idStr, out var studentId)) return Unauthorized();

            var items = await _svc.ListMineAsync(studentId, ct);
            return Ok(items);
        }

        //GET /api/students/me/transcript
        [HttpGet("transcript")]
        public async Task<IActionResult> Transcript(CancellationToken ct)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(idStr, out var studentId)) return Unauthorized();

            var items = await _progressSvc.GetMyTranscriptAsync(studentId, ct);
            return Ok(items);
        }

    }

}
