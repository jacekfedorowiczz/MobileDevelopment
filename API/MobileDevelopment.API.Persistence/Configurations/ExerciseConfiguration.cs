using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    internal sealed class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(e => e.Description)
                .HasMaxLength(1024);

            builder.Property(e => e.ImageUrl)
                .HasMaxLength(2048);

            builder.Property(e => e.Difficulty)
                .HasConversion<int?>();

            builder.Property(e => e.IsCompound)
                .IsRequired();

            builder.HasIndex(e => e.Name).IsUnique();
        }
    }
}

