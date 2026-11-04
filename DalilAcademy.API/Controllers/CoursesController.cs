using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")] // Route attribute
    [ApiController] // marks the class as an API controller (adds helpful API defaults, like returning 400 errors automatically when input is invalid)
    public class CoursesController(ICourseService courseService) : ControllerBase // Primary Contstructor 
    {
        [HttpGet]
        [Route("count")]
        public async Task<ActionResult<int>> GetAllCount()
        {
            return Ok(await courseService.GetCountAsync());
        }

        [HttpGet]
        [Route("all")]
        // not having a Route attribute means that it will pull the controller Route attribute
        //[Route("get-all-courses")] // https:localhost/port#/api/courses/get-all-courses (if u wanted a custom url)
        public async Task<ActionResult<List<CourseResponseDto>>> GetAll()
        {
            return Ok(await courseService.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<CourseResponseDto>>> GetAllByPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await courseService.GetPageAsync(page, pageSize));
        }

        [HttpGet]
        [Route("filter-by-price")]
        public async Task<ActionResult<PageResult<CourseResponseDto>>> FilterByPricePage([FromQuery] int min = 0, [FromQuery] int max = 9999, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            min = Math.Max(min, 0);
            max = Math.Min(max, 9999);
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await courseService.GetByPricePageAsync(min, max, page, pageSize));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<CourseResponseDto>> GetById(int id)
        {
            var course = await courseService.GetByIdAsync(id);
            if (course is null)
                return NotFound();

            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<CourseResponseDto>> Create(CreateCourseDto dto)
        {
            //try
            //{

            var created = await courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            //}
            //catch(FluentValidation.ValidationException ex)
            //{
            //    //return BadRequest(ex.Message);
            //    return BadRequest(ex.Errors.Select(e => new {e.PropertyName, e.ErrorMessage}));
            //}
            //catch (ArgumentException ex)
            //{
            //    return BadRequest(ex.Message);
            //}
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Update(int id, UpdateCourseDto course)
        {
            await courseService.UpdateAsync(id, course);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await courseService.DeleteAsync(id);
            return NoContent();
        }

        // run the project, then test the API URLs on Postman to check if the code works
    }
}