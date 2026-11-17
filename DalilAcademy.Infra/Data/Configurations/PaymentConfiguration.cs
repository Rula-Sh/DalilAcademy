using DalilAcademy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DalilAcademy.Infra.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.PayPalOrderId).IsUnique();

            builder.HasOne(p => p.Enrollment)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Amount).IsRequired().HasPrecision(10, 2);

            builder.Property(p => p.Currency).IsRequired().HasDefaultValue("USD");

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(PaymentStatus.Created);

            builder.Property(p => p.CreatedAt).HasDefaultValueSql("now()");
        }
    }
}
