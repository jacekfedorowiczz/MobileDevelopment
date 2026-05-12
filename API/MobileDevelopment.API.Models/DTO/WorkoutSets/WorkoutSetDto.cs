namespace MobileDevelopment.API.Models.DTO.WorkoutSets
{
    public sealed record WorkoutSetDto(
        int Id,
        int WorkoutSessionId,
        int ExerciseId,
        int SetNumber,
        decimal Weight,
        int Reps,
        int? Rpe,
        MobileDevelopment.API.Models.DTO.WorkoutSessions.WorkoutSessionDto? WorkoutSession = null,
        MobileDevelopment.API.Models.DTO.Exercises.ExerciseDto? Exercise = null
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
