using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController(IStudentService studentService) : ControllerBase
    {
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<Student>>> GetAll()
        {
            return Ok(await studentService.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<StudentResponseDto>>> GetAllByPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await studentService.GetPageAsync(page, pageSize));
        }


        [HttpGet]
        [Route("{id:int}/courses")]
        public async Task<ActionResult<PageResult<CourseResponseDto>>> GetCoursesInPage(int id,[FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await studentService.GetCoursesInPageAsync(id, page, pageSize));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<StudentResponseDto>> GetById(int id)
        {
            var student = await studentService.GetByIdAsync(id);
            if (student is null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> Create(CreateStudentDto dto)
        {
            var created = await studentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Update(int id, UpdateStudentDto student)
        {
            await studentService.UpdateAsync(id, student);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await studentService.DeleteAsync(id);
            return NoContent(); ;
        }
    }
}
