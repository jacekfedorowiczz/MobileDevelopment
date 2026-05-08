namespace MobileDevelopment.API.Models.DTO.Exercises
{
    public sealed record ExerciseDto(
        int Id,
        string Name,
        string? Description,
        bool IsCompound
    );

    public sealed record CreateEditExerciseDto(
        string Name,
        string? Description,
        bool IsCompound,
        IEnumerable<int> MuscleGroupIds
    );
}
