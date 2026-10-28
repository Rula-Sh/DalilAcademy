using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController(IStudentService studentService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAll()
        {
            return Ok(await studentService.GetAllAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await studentService.GetByIdAsync(id);
            if (student is null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> Create(Student student)
        {
            try
            {
                var created = await studentService.CreateAsync(student);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Update(int id, Student student)
        {
            try
            {
                if (id != student.Id)
                    return BadRequest();

                return await studentService.UpdateAsync(id, student) ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await studentService.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
