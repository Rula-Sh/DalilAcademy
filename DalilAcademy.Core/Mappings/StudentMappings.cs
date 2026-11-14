using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Mappings
{
    public static class StudentMappings
    {
        public static StudentResponseDto ToDto(this Student student)
        {
            return new StudentResponseDto(student.Id, student.FullName, student.User.Email, student.Level);
        }
        public static Student ToEntity(this CreateStudentDto dto, User user)
        {
            return new Student
            {
                FullName = dto.FullName,
                User = user
            };
        }
    }
}
