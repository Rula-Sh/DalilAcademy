using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class InstructorsController(IInstructorService InstructorService, IAuthService authService) : ControllerBase
    {
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<Instructor>>> GetAll()
        {
            return Ok(await InstructorService.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<InstructorResponseDto>>> GetAllByPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await InstructorService.GetPageAsync(page, pageSize));
        }

        [HttpGet]
        [Route("{id:int}/courses")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Instructor}")]
        public async Task<ActionResult<PageResult<CourseResponseDto>>> GetCoursesInPage(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await InstructorService.GetCoursesInPageAsync(id, page, pageSize));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<InstructorResponseDto>> GetById(int id)
        {
            var Instructor = await InstructorService.GetByIdAsync(id);
            if (Instructor is null)
                return NotFound();

            return Ok(Instructor);
        }

        [HttpPost]
        public async Task<ActionResult<InstructorResponseDto>> Create(CreateInstructorDto dto)
        {
            var created = await authService.RegisterInstructorByAdminAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Update(int id, UpdateInstructorDto Instructor)
        {
            await InstructorService.UpdateAsync(id, Instructor);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await InstructorService.DeleteAsync(id);
            return NoContent(); ;
        }
    }
}
