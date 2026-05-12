namespace MobileDevelopment.API.Models.DTO.MuscleGroups
{
    public sealed record MuscleGroupDto(
        int Id,
        string Name,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.Exercises.ExerciseDto>? Exercises = null
    );

    public sealed record CreateEditMuscleGroupDto(
        string Name
    );
}
