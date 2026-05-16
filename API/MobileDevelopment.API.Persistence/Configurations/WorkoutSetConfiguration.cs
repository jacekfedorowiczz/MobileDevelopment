using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    internal sealed class WorkoutSetConfiguration : IEntityTypeConfiguration<WorkoutSet>
    {
        public void Configure(EntityTypeBuilder<WorkoutSet> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SetNumber)
                .IsRequired();

            builder.Property(s => s.Weight)
                .HasPrecision(6, 2);

            builder.Property(s => s.Reps)
                .IsRequired();

            builder.HasOne(s => s.WorkoutSession)
                .WithMany(ws => ws.Sets)
                .HasForeignKey(s => s.WorkoutSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Exercise)
                .WithMany(e => e.Sets)
                .HasForeignKey(s => s.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

