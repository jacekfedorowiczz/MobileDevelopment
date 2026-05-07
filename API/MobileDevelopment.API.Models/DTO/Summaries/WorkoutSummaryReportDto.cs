namespace MobileDevelopment.API.Models.DTO.Summaries
{
    public sealed record WorkoutSummaryReportDto(int SessionId, decimal TotalVolumeKilos, string FatigueStatus, List<ExeciseMaxRecordDto> Estimated1RMs)
    {
    }

    public sealed record ExeciseMaxRecordDto(string ExerciseName, decimal EstimatedOneRM);
}
