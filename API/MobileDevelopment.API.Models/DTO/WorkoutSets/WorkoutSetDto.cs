namespace MobileDevelopment.API.Models.DTO.WorkoutSets
{
    public sealed record WorkoutSetDto(
        int Id,
        int WorkoutSessionId,
        int ExerciseId,
        int SetNumber,
        decimal Weight,
        int Reps,
        int? Rpe
    );

    public sealed record CreateEditWorkoutSetDto(
        int WorkoutSessionId,
        int ExerciseId,
        int SetNumber,
        decimal Weight,
        int Reps,
        int? Rpe
    );
}
