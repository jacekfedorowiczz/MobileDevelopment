using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.DTO.WorkoutSets;

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
        UserDto? User = null,
        ICollection<WorkoutSetDto>? Sets = null
    );

    public sealed record WorkoutSessionSummaryDto(
        int Id,
        int UserId,
        string Name,
        string? Description,
        DateTime StartTime,
        DateTime? EndTime,
        int? GlobalSessionRpe,
        int ExerciseCount,
        int SetCount
    );

    public sealed record CreateEditWorkoutSessionDto(
        string Name,
        string? Description,
        DateTime StartTime,
        DateTime? EndTime
    );

    public sealed record CreateWorkoutSessionWithSetsDto(
        string Name,
        string? Description,
        DateTime StartTime,
        DateTime? EndTime,
        IEnumerable<CreateEditWorkoutSetDto>? Sets = null
    );
}

