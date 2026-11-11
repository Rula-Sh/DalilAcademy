namespace DalilAcademy.Core.DTOs
{
    public record CreateCourseDto(string Title, decimal Price, int InstructorId, string? ImageUrl);
    public record UpdateCourseDto(string Title, decimal Price, string? ImageUrl);
    public record CourseResponseDto(int Id, string Title, decimal Price, string? InstructorName, string? ImageUrl);
    public record ImageUploadResponseDto(string? ImageUrl);
    public record CourseWithEnrollmentsDto(int Id, string Title, string? InstructorName, List<CourseEnrollmentSummaryDto>? Enrollments);
    public record CourseEnrollmentSummaryDto(int StudentId, string FullName, string Email, DateTime EnrolledAt);    
}