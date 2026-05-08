using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Models.DTO.Profiles
{
    public sealed record ProfileDto(
        int Id,
        int UserId,
        int Age,
        decimal Weight,
        decimal Height,
        WeightUnits PreferredWeightUnit,
        bool IsDarkModeEnabled,
        FitnessGoal CurrentGoal
    );

    public sealed record CreateEditProfileDto(
        int UserId,
        decimal Weight,
        decimal Height,
        WeightUnits PreferredWeightUnit,
        bool IsDarkModeEnabled,
        FitnessGoal CurrentGoal
    );
}
