using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    public class ProfileAchievementConfiguration : IEntityTypeConfiguration<ProfileAchievement>
    {
        public void Configure(EntityTypeBuilder<ProfileAchievement> builder)
        {
            builder.HasKey(pa => pa.Id);

            builder.HasIndex(pa => new { pa.ProfileId, pa.AchievementId }).IsUnique();

            builder.HasOne(pa => pa.Profile)
                .WithMany(p => p.ProfileAchievements)
                .HasForeignKey(pa => pa.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pa => pa.Achievement)
                .WithMany(a => a.ProfileAchievements)
                .HasForeignKey(pa => pa.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
