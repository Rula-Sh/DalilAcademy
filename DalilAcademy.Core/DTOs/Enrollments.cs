namespace DalilAcademy.Core.DTOs
{
    public record CreateEnrollmentDto(int? StudentId, int? CourseId);
    public record UpdateEnrollmentDto(int? StudentId, int? CourseId);
    public record EnrollmentResponseDto(int Id, string? StudentName, string? CourseName, DateTime EnrolledAt, string Status);
    public record StudentEnrollmentsResponseDto(string? CourseName, DateTime EnrolledtAt);
}
