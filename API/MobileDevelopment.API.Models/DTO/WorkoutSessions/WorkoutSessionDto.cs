namespace MobileDevelopment.API.Models.DTO.WorkoutSessions
{
    public sealed record WorkoutSessionDto(
        int Id,
        int UserId,
        string Name,
        string? Description,
        DateTime StartTime,
        DateTime? EndTime,
        int? GlobalSessionRpe,
        MobileDevelopment.API.Models.DTO.Users.UserDto? User = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.WorkoutSets.WorkoutSetDto>? Sets = null
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
