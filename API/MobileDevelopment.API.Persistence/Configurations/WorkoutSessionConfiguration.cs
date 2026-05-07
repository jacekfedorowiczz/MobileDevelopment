using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    internal sealed class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
    {
        public void Configure(EntityTypeBuilder<WorkoutSession> builder)
        {
            builder.HasKey(ws => ws.Id);

            builder.Property(ws => ws.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(ws => ws.Description)
                .HasMaxLength(1024);

            builder.Property(ws => ws.StartTime)
                .IsRequired();

            builder.HasOne(ws => ws.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(ws => ws.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

