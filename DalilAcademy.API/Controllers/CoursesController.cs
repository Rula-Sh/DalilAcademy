using DalilAcademy.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")] // Route attribute
    [ApiController] // marks the class as an API controller (adds helpful API defaults, like returning 400 errors automatically when input is invalid)
    public class CoursesController : ControllerBase
    {
        // CRUD Operations

        // No Database for now, use a Temporary Storage for now (Replace it with a real DB later)
        private static readonly List<Course> _courses = []; // static keeps the list alive

        // GetAll
        [HttpGet]
        // not having a Route attribute means that it will pull the controller Route attribute
        //[Route("get-all-courses")] // https:localhost/port#/api/courses/get-all-courses (if u wanted a custom url)
        public ActionResult<List<Course>> GetAll()
        {
            return Ok(_courses);
        }

        // GetById
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Course> GetById(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            return course is null ? NotFound() : Ok(course);
        }

        // Create
        [HttpPost]
        public ActionResult<Course> Create(Course course)
        {
            course.Id = _courses.Count == 0 ? 1 : _courses.Max(c => c.Id) + 1;
            _courses.Add(course);
            //return course;
            //CreatedAtAction returns 201 Created plus a Location header pointing to the new course.
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        // Update
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<Course> Update(Course course)
        {
            var courseIndex = _courses.FindIndex(c => c.Id == course.Id);
            if (courseIndex == -1)
                return NotFound();

            _courses[courseIndex] = course;
            return course;
        }

        // Delete
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult<Course> Delete(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            _courses.Remove(course);
            return NoContent(); // Returns HTTP 204 (No Content)
        }

        // run the project, then test the API URLs on Postman to check if the code works
    }
}