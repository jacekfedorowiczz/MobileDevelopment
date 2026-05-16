using MobileDevelopment.API.Domain.Base;
using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Achievement : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconCode { get; set; } = string.Empty;
        public AchievementType AchievementType { get; set; }
        public int TargetValue { get; set; }
        public ICollection<ProfileAchievement> ProfileAchievements { get; set; } = new List<ProfileAchievement>();
    }
}
