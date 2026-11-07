using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
    {
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<EnrollmentResponseDto>>> GetAll()
        {
            return Ok(await enrollmentService.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<EnrollmentResponseDto>>> GetAllByPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await enrollmentService.GetPageAsync(page, pageSize));
        }

        [HttpGet]
        [Route("me")]
        [Authorize(Roles = Roles.Student)]
        public async Task<ActionResult<List<StudentEnrollmentsResponseDto>>> GetStudentEnrollments([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {

            string? token = await HttpContext.GetTokenAsync("access_token");

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (userId is 0 || token is null)
                new UnauthorizedException("You are not logged in.");

            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await enrollmentService.GetStudentEnrollmentsAsync(userId, page, pageSize));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<EnrollmentResponseDto>> GetById(int id)
        {
            var enrollment = await enrollmentService.GetByIdAsync(id);
            if (enrollment is null)
                return NotFound();

            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDto>> Enroll(CreateEnrollmentDto dto)
        {
            var created = await enrollmentService.EnrollAsync(dto.StudentId, dto.CourseId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id}, created);            
        }
    }
}
