using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(a => a.Description)
                .HasMaxLength(512);

            builder.Property(a => a.IconCode)
                .HasMaxLength(64);

            builder.Property(a => a.AchievementType)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(a => a.TargetValue)
                .IsRequired();
        }
    }
}
