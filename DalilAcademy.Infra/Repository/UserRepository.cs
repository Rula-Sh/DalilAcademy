using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace DalilAcademy.Infra.Repository
{
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        public Task<User?> GetByEmailAsync(string email) => 
            db.Users
            .AsNoTracking() // used to stop tracking the object to update it incase any changes occurred on it, it saves more memory and time in large databases
            .FirstOrDefaultAsync(u => u.Email == email);        
    }
}