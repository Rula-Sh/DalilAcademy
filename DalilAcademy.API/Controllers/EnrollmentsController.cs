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
        public async Task<ActionResult<List<EnrollmentResponseDto>>> GetAll()
        {
            return Ok(await enrollmentService.GetAllAsync());
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
