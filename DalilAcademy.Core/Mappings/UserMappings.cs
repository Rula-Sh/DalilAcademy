using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Mappings
{
    public static class UserMappings
    {
        public static UserResponseDto ToDto(this User user)
        {
            return new UserResponseDto(user.Email);
        }
        public static User ToUserEntity(this CreateStudentDto dto, string passwordHash)
        {
            return new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash
            };
        }
        public static User ToUserEntity(this Student dto)
        {
            return new User
            {
                Email = dto.Email,
                PasswordHash = dto.User!.PasswordHash,
                RequireUpdatingPassword = dto.User.RequireUpdatingPassword
            };
        }
    }
}
