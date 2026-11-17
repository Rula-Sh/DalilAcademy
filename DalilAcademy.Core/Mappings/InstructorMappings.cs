using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Mappings
{
    public static class InstructorMappings
    {
        public static InstructorResponseDto ToDto(this Instructor Instructor)
        {
            return new InstructorResponseDto(Instructor.Id, Instructor.FullName, Instructor.User.Email);
        }
        public static Instructor ToEntity(this CreateInstructorDto dto, User user)
        {
            return new Instructor
            {
                FullName = dto.FullName,
                User = user
            };
        }
    }
}
