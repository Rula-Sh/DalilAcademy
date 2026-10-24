using DalilAcademy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DalilAcademy.Infra.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FullName).IsRequired().HasMaxLength(200);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(256);
            builder.HasIndex(s => s.Email).IsUnique();
            builder.HasMany(s => s.Enrollments).WithOne(e => e.Student).HasForeignKey(e => e.StudentId);
        }
    }
}
