using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
    }
}
