using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entites;
namespace DalilAcademy.Core.Mappings
{
    public static class CourseMappings
    {
        public static CourseResponseDto ToDto(this Course course) // to make it an extention method, make the class and method static, and add this in the method parameter
        {
            return new CourseResponseDto(course.Id, course.Title, course.Price, course.Instructor?.FullName);
        }
        public static Course ToEntity(this CreateCourseDto dto)
        {
            return new Course()
            {
                Title = dto.Title,
                Price = dto.Price,
                InstructorId = dto.InstructorId
            };
        }
    }
}
