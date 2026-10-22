using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")] // Route attribute
    [ApiController] // marks the class as an API controller (adds helpful API defaults, like returning 400 errors automatically when input is invalid)
    public class CoursesController(ICourseService courseService) : ControllerBase // Primary Contstructor 
    {
        // CRUD Operations

        // GetAll
        [HttpGet]
        // not having a Route attribute means that it will pull the controller Route attribute
        //[Route("get-all-courses")] // https:localhost/port#/api/courses/get-all-courses (if u wanted a custom url)
        public ActionResult<List<Course>> GetAll()
        {
            return Ok(courseService.GetAll());
        }

        // GetById
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Course> GetById(int id)
        {
            return Ok(courseService.GetById(id));
        }

        // Create
        [HttpPost]
        public ActionResult<Course> Create(Course course)
        {
            return courseService.Create(course);
        }

        // Update
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<Course> Update(int id, Course course)
        {
            if (id != course.Id)
                return BadRequest();

            var updated = courseService.Update(course);
            if (updated is null)
                return NotFound();

            return Ok(updated);
        }

        // Delete
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            var deleted = courseService.Delete(id);
            if (deleted is null)
                return NotFound();

            return NoContent();
        }

        // run the project, then test the API URLs on Postman to check if the code works
    }
}