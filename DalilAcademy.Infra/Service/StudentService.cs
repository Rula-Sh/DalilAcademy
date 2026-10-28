using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;

namespace DalilAcademy.Infra.Service
{
    public class StudentService(IStudentRepository studentRepository) : IStudentService
    {
        public Task<List<Student>> GetAllAsync()
        {
            return studentRepository.GetAllAsync();
        }
        public Task<Student?> GetByIdAsync(int id)
        {
            return studentRepository.GetByIdAsync(id);
        }
        public Task<Student> CreateAsync(Student student)
        {

            if (student.Level <= 0)
                throw new ArgumentException("Student level cannot be 0 or below");

            return studentRepository.CreateAsync(student);
        }
        public async Task<bool> UpdateAsync(int id, Student input)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student is null)
                return false;

            student.FullName = input.FullName;
            student.Email = input.Email;
            student.Level = input.Level;

            await studentRepository.UpdateAsync(student);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await studentRepository.DeleteAsync(id);
        }
    }
}
