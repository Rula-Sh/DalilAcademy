using DalilAcademy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DalilAcademy.Infra.Data.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.FullName).IsRequired().HasMaxLength(200);

            builder.HasIndex(i => i.UserId).IsUnique();
            builder.HasOne(i => i.User)
               .WithOne()
               .HasForeignKey<Instructor>(i => i.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
