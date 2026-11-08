namespace DalilAcademy.Core.DTOs
{
    public record CreateCourseDto(string Title, decimal Price, int InstructorId, string? ImageUrl);
    public record UpdateCourseDto(string Title, decimal Price, string? ImageUrl);
    public record CourseResponseDto(int Id, string Title, decimal Price, string? InstructorName, string? ImageUrl);
    public record ImageUploadResponseDto(string? ImageUrl);
}