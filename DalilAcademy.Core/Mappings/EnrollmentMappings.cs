using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Mappings
{
    public static class EnrollmentMappings
    {
        public static EnrollmentResponseDto ToDto(this Enrollment enrollment) // to make it an extention method, make the class and method static, and add this in the method parameter
        {
            return new EnrollmentResponseDto(enrollment.Id, enrollment.Student?.FullName, enrollment.Course?.Title, enrollment.EnrolledAt);
        }
        public static Enrollment ToEntity(this CreateEnrollmentDto dto)
        {
            return new Enrollment()
            {
                StudentId = dto.StudentId!.Value,
                CourseId = dto.CourseId!.Value,
                EnrolledAt = DateTime.UtcNow
            };
        }
        public static Enrollment ToEntity(this UpdateEnrollmentDto dto)
        {
            return new Enrollment()
            {
                StudentId = dto.StudentId!.Value,
                CourseId = dto.CourseId!.Value,
                EnrolledAt = DateTime.UtcNow
            };
        }
    }
}
