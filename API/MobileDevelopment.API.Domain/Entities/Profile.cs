using MobileDevelopment.API.Domain.Base;
using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Profile : BaseEntity
    {
        public int UserId { get; set; }
        public int Age { get; set; }
        public string? Avatar { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public required WeightUnits PreferredWeightUnit { get; set; } = WeightUnits.Kilos;
        public User User { get; set; } = null!;

        public FitnessGoal CurrentGoal { get; set; } = FitnessGoal.Maintain;
        public string? DietType { get; set; }
        public int? DailyCaloriesGoal { get; set; }
        public int? ProteinPercentage { get; set; }
        public int? CarbsPercentage { get; set; }
        public int? FatPercentage { get; set; }
        public ICollection<Tag> Interests { get; set; } = [];
        public ICollection<ProfileAchievement> ProfileAchievements { get; set; } = [];
    }
}
