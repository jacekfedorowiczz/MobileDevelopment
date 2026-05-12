namespace MobileDevelopment.API.Models.DTO.Exercises
{
    public sealed record ExerciseDto(
        int Id,
        string Name,
        string? Description,
        bool IsCompound,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.MuscleGroups.MuscleGroupDto>? TargetedMuscles = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.WorkoutSets.WorkoutSetDto>? Sets = null
    );

    public sealed record CreateEditExerciseDto(
        string Name,
        string? Description,
        bool IsCompound,
        IEnumerable<int> MuscleGroupIds
    );
}
