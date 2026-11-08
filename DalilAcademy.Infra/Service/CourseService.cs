using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using DalilAcademy.Core.Storage;
using FluentValidation;

namespace DalilAcademy.Infra.Service
{
    public class CourseService(ICourseRepository courseRepository, IValidator<CreateCourseDto> validator, IImageService images) : ICourseService
    {
        public Task<int> GetCountAsync()
        {
            return courseRepository.GetCountAsync();
        }
        public Task<List<CourseResponseDto>> GetAllAsync()
        {
            //var courses = await courseRepository.GetAllAsync();
            //return courses.Select(c => new CourseResponseDto(c.Id, c.Title, c.Price, c.Instructor?.FullName)).ToList(); // this code will repeat in multiple times, and incase of an update on the CourseResponseDto, i will need to update it everywhere, that's why we will use Extension-Method Mapping
            //return courses.Select(c => CourseMappings.ToDto(c)).ToList(); // before making the ToDto an extention method completely (added the static, but not the this in the parameter)
            //return courses.Select(c => c.ToDto()).ToList(); // no longer needed, since i returned the needed data in CourseRepository
            return courseRepository.GetAllAsync();
        }
        public Task<PageResult<CourseResponseDto>> GetPageAsync(int page, int pageSize)
        {
            return courseRepository.GetPageAsync(page, pageSize);
        }
        public Task<PageResult<CourseResponseDto>> GetByPricePageAsync(int min, int max, int page, int pageSize)
        {
            return courseRepository.GetByPricePageAsync(min, max, page, pageSize);
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
        public async Task UpdateAsync(int id, UpdateCourseDto input)
        {
            var course = await courseRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Course with ID {id} was not found");

            course.Title = input.Title;
            course.Price = input.Price;

            var oldImage = course.ImageUrl;
            course.Title = input.Title; course.Price = input.Price; course.ImageUrl = input.ImageUrl;
            await courseRepository.UpdateAsync(course);

            if (oldImage != input.ImageUrl)
                images.DeleteCourseImage(oldImage);
        }
        public async Task DeleteAsync(int id)
        {
            var course = await courseRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Course with ID {id} was not found");
            await courseRepository.DeleteAsync(id);
            images.DeleteCourseImage(course.ImageUrl);
        }
    }
}
