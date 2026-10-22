using DalilAcademy.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalilAcademy.Core.Repository
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course GetById(int id);
        Course Create(Course course);
        Course Update(Course course);
        Course Delete(int id);
    }
}
