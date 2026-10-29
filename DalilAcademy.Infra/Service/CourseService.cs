using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using DalilAcademy.Core.Validators;
using FluentValidation;

namespace DalilAcademy.Infra.Service
{
    public class CourseService(ICourseRepository courseRepository, IValidator<CreateCourseDto> validator) : ICourseService
    {
        public async Task<List<CourseResponseDto>> GetAllAsync()
        {
            var courses = await courseRepository.GetAllAsync();
            //return courses.Select(c => new CourseResponseDto(c.Id, c.Title, c.Price, c.Instructor?.FullName)).ToList(); // this code will repeat in multiple times, and incase of an update on the CourseResponseDto, i will need to update it everywhere, that's why we will use Extension-Method Mapping
            //return courses.Select(c => CourseMappings.ToDto(c)).ToList(); // before making the ToDto an extention method completely (added the static, but not the this in the parameter)
            return courses.Select(c => c.ToDto()).ToList();
        }
        public async Task<CourseResponseDto?> GetByIdAsync(int id)
        {
            var course = await courseRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Course with ID {id} was not found");
            return course?.ToDto();
        }
        public async Task<CourseResponseDto> CreateAsync(CreateCourseDto dto)
        {
            await validator.ValidateAndThrowAsync(dto); // validates the dto data, else throws a validation exception

            var course = dto.ToEntity();
            var created = await courseRepository.CreateAsync(course);

            return created.ToDto();
        }
        public async Task UpdateAsync(int id, Course input)
        {
            var course = await courseRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Course with ID {id} was not found");

            course.Title = input.Title;
            course.Price = input.Price;

            await courseRepository.UpdateAsync(course);
        }
        public async Task DeleteAsync(int id)
        {
            if (!await courseRepository.DeleteAsync(id))
                throw new NotFoundException($"Course with ID {id} was not found");
        }
    }
}
