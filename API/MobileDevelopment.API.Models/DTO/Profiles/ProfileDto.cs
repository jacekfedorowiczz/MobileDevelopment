using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.DTO.Users;

namespace MobileDevelopment.API.Models.DTO.Profiles
{
    public sealed record ProfileDto(
        int Id,
        int UserId,
        string? Avatar,
        int Age,
        decimal Weight,
        decimal Height,
        WeightUnits PreferredWeightUnit,
        FitnessGoal CurrentGoal,
        string? DietType = null,
        int? DailyCaloriesGoal = null,
        int? ProteinPercentage = null,
        int? CarbsPercentage = null,
        int? FatPercentage = null,
        UserDto? User = null,
        ICollection<TagDto>? Interests = null
    );

    public sealed record CreateEditProfileDto(
        int? UserId = null,
        string? FirstName = null,
        string? LastName = null,
        string? Email = null,
        string? Avatar = null,
        decimal? Weight = null,
        decimal? Height = null,
        WeightUnits? PreferredWeightUnit = null,
        FitnessGoal? CurrentGoal = null,
        string? DietType = null,
        int? DailyCaloriesGoal = null,
        int? ProteinPercentage = null,
        int? CarbsPercentage = null,
        int? FatPercentage = null
    );

    public sealed record MyProfileDto(
        string FirstName,
        string LastName,
        string Email,
        string? ProfileImageUrl,
        int WorkoutsThisMonth,
        decimal AverageWorkoutTime,
        int AchievementsCount,
        decimal Weight,
        decimal Height,
        int CurrentGoal,
        int PreferredWeightUnit,
        string? DietType,
        int? DailyCaloriesGoal,
        int? ProteinPercentage,
        int? CarbsPercentage,
        int? FatPercentage);
}
