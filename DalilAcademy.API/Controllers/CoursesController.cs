using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")] // Route attribute
    [ApiController] // marks the class as an API controller (adds helpful API defaults, like returning 400 errors automatically when input is invalid)
    public class CoursesController(ICourseService courseService) : ControllerBase // Primary Contstructor 
    {

        [HttpGet]
        // not having a Route attribute means that it will pull the controller Route attribute
        //[Route("get-all-courses")] // https:localhost/port#/api/courses/get-all-courses (if u wanted a custom url)
        public async Task<ActionResult<List<Course>>> GetAll()
        {
            return Ok(await courseService.GetAllAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Course>> GetById(int id)
        {
            var course = await courseService.GetByIdAsync(id);
            if (course is null)
                return NotFound();

            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<Course>> Create(Course course)
        {
            try
            {
                var created = await courseService.CreateAsync(course);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Update(int id, Course course)
        {
            try
            {
                if (id != course.Id)
                    return BadRequest();

                return await courseService.UpdateAsync(id, course) ? NoContent() : NotFound();
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
            return await courseService.DeleteAsync(id) ? NoContent() : NotFound();
        }

        // run the project, then test the API URLs on Postman to check if the code works
    }
}