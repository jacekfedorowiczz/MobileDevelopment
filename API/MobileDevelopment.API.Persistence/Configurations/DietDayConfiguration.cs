using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    internal sealed class DietDayConfiguration : IEntityTypeConfiguration<DietDay>
    {
        public void Configure(EntityTypeBuilder<DietDay> builder)
        {
            builder.HasKey(dd => dd.Id);

            builder.Property(dd => dd.Date)
                .IsRequired();

            builder.Property(dd => dd.Notes)
                .HasMaxLength(1024);

            builder.HasOne(dd => dd.Diet)
                .WithMany(d => d.DietDays)
                .HasForeignKey(dd => dd.DietId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
