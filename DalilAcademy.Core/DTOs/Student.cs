using System;
using System.Collections.Generic;
using System.Text;

namespace DalilAcademy.Core.DTOs
{
    public record CreateStudentDto(string FullName, string Email);
    public record UpdateStudentDto(string FullName, string Email);
    public record StudentResponseDto(int Id, string FullName, string Email, int Level);
}
