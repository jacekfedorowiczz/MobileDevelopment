using MobileDevelopment.API.Domain.Base;
using System;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class ProfileAchievement : BaseEntity
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
        public int AchievementId { get; set; }
        public Achievement Achievement { get; set; } = null!;
        public DateTime UnlockedAt { get; set; }
    }
}
