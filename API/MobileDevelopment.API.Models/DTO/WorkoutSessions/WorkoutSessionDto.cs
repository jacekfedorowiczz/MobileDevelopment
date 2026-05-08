namespace MobileDevelopment.API.Models.DTO.WorkoutSessions
{
    public sealed record WorkoutSessionDto(
        int Id,
        int UserId,
        string Name,
        string? Description,
        DateTime StartTime,
        DateTime? EndTime,
        int? GlobalSessionRpe
    );

    public sealed record CreateEditWorkoutSessionDto(
        int UserId,
        string Name,
        string? Description,
        DateTime StartTime,
        DateTime? EndTime,
        int? GlobalSessionRpe
    );
}
