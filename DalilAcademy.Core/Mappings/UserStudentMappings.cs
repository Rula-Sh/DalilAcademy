using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Mappings
{
    public static class UserStudentMappings
    {
        public static UserResponseDto ToDto(this User user)
        {
            return new UserResponseDto(user.Email);
        }

        public static Student ToStudentEntity(this RegisterStudentDto dto)
        {
            return new Student
            {
                FullName = dto.FullName,
                Email = dto.Email
            };
        }
        public static User ToUserEntity(this RegisterStudentDto dto)
        {
            return new User
            {
                Email = dto.Email,
                PasswordHash = dto.Password
            };
        }
    }
}
