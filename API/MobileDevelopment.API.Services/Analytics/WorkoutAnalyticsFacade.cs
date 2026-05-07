using MobileDevelopment.API.Models.DTO.Summaries;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Services.Calculators;
using MobileDevelopment.API.Services.Services.Facades;

namespace MobileDevelopment.API.Services.Analytics
{
    internal sealed class WorkoutAnalyticsFacade : IWorkoutAnalyticsFacade
    {
        private readonly IVolumeCalculator _volumeCalculator;
        private readonly IOneRepMaxCalculator _oneRepMaxCalculator;
        private readonly IFatigueCalculator _fatigueAnalyzer;
        private readonly IWorkoutSessionService _sessionService;

        public WorkoutAnalyticsFacade(IVolumeCalculator volumeCalculator, IOneRepMaxCalculator oneRepMaxCalculator, IFatigueCalculator fatigueAnalyzer, IWorkoutSessionService sessionService)
        {
            _volumeCalculator = volumeCalculator;   
            _oneRepMaxCalculator = oneRepMaxCalculator;
            _fatigueAnalyzer = fatigueAnalyzer;
            _sessionService = sessionService;
        }

        public async Task<WorkoutSummaryReportDto> GenerateSessionSummaryAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            //var session = await _sessionService.GetSessionById(sessionId) 
            //    ?? throw new InvalidOperationException($"Cannot generate summary for the session with id {sessionId}");

            //var totalVolume = _volumeCalculator.CalculateVolume(session.Sets);
            //var avgRpe = session.Sets.Average(x => (double?)x.Rpe) ?? 0.0;
            //var fatigueStatus = _fatigueAnalyzer.Analyze(session.GlobalSessionRpe, avgRpe);

            //var oneRepsMax = session.Sets
            //    .GroupBy(x => x.Exercise.Name)
            //    .Select(g => new ExeciseMaxRecordDto(g.Key, g.Max(x => _oneRepMaxCalculator.Calculate(x.Weight, x.Reps))))
            //    .ToList();

            //return new WorkoutSummaryReportDto(sessionId, totalVolume, fatigueStatus, oneRepsMax);
            throw new NotImplementedException();
        }
    }
}
