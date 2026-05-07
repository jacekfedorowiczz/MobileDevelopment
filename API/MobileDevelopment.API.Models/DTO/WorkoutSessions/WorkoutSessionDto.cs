namespace MobileDevelopment.API.Models.DTO.WorkoutSessions
{
    public record WorkoutSessionDto(
            int Id,
            string Name,
            DateTime StartTime,
            DateTime? EndTime,
            int? GlobalSessionRpe,
            int TotalSetsCount
        );
}
