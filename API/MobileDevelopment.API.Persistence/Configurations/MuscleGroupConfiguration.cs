using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    internal sealed class MuscleGroupConfiguration : IEntityTypeConfiguration<MuscleGroup>
    {
        public void Configure(EntityTypeBuilder<MuscleGroup> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(64);

            builder.HasIndex(m => m.Name).IsUnique();

            builder.HasMany(m => m.Exercises)
                .WithMany(e => e.TargetedMuscles)
                .UsingEntity(j => j.ToTable("ExerciseMuscleGroups"));
        }
    }
}

