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
        UserDto? User = null,
        ICollection<TagDto>? Interests = null
    );

    public sealed record CreateEditProfileDto(
        int UserId,
        string? Avatar,
        decimal Weight,
        decimal Height,
        WeightUnits PreferredWeightUnit,
        FitnessGoal CurrentGoal
    );
}
