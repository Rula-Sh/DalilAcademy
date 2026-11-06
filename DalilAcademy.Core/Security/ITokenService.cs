using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Security
{
    public interface ITokenService
    {
         (string Token, DateTime ExpiresAt) CreateToken(User user); // the return type is a tuple, a named tuple
    }
}
