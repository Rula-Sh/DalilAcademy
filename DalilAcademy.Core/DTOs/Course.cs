namespace DalilAcademy.Core.DTOs
{
    public record CreateCourseDto(string Title, decimal Price, int InstructorId);
    public record UpdateCourseDto(string Title, decimal Price);
    public record CourseResponseDto(int Id, string Title, decimal Price, string? InstructorName);
}