using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalilAcademy.Core.Mappings
{
    public static class StudentMappings
    {
        public static StudentResponseDto ToDto(this Student student)
        {
            return new StudentResponseDto(student.Id, student.FullName, student.Email, student.Level);
        }
        public static Student ToEntity(this CreateStudentDto dto)
        {
            return new Student
            {
                FullName = dto.FullName,
                Email = dto.Email
            };
        }
    }
}
