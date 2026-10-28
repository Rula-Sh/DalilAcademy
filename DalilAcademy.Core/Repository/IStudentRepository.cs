using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student> CreateAsync(Student student);
        Task UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
    }
}
