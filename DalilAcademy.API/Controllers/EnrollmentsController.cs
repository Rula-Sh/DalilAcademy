using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
