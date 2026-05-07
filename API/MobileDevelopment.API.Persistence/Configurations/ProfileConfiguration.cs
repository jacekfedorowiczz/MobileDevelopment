using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    internal sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Age)
                .IsRequired();

            builder.Property(p => p.Weight)
                .HasPrecision(6, 2);

            builder.Property(p => p.Height)
                .HasPrecision(5, 2);

            builder.Property(p => p.PreferredWeightUnit)
                .HasConversion<string>()
                .HasMaxLength(16);

            builder.Property(p => p.CurrentGoal)
                .HasConversion<string>()
                .HasMaxLength(32);

            builder.HasMany(p => p.Interests)
                .WithMany(t => t.InterestedProfiles)
                .UsingEntity(j => j.ToTable("ProfileTags"));
        }
    }
}

