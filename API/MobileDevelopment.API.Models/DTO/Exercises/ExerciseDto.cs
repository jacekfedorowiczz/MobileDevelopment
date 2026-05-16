using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.DTO.WorkoutSets;

namespace MobileDevelopment.API.Models.DTO.Exercises
{
    public sealed record ExerciseDto(
        int Id,
        string Name,
        string? Description,
        bool IsCompound,
        bool IsSystem = false,
        int? CreatedByUserId = null,
        string? ImageUrl = null,
        ExerciseDifficulty? Difficulty = null,
        ICollection<MuscleGroupDto>? TargetedMuscles = null,
        ICollection<WorkoutSetDto>? Sets = null
    );

    public sealed record CreateEditExerciseDto(
        string Name,
        string? Description,
        bool IsCompound,
        IEnumerable<int> MuscleGroupIds,
        string? ImageUrl = null,
        ExerciseDifficulty? Difficulty = null
    );
}
