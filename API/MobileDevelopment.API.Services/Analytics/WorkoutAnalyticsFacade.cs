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
        private readonly IWorkoutSessionService _sessionService;

        public WorkoutAnalyticsFacade(IVolumeCalculator volumeCalculator, IOneRepMaxCalculator oneRepMaxCalculator, IWorkoutSessionService sessionService)
        {
            _volumeCalculator = volumeCalculator;   
            _oneRepMaxCalculator = oneRepMaxCalculator;
            _sessionService = sessionService;
        }

        public async Task<WorkoutSummaryReportDto> GenerateSessionSummaryAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            // TODO 
            throw new NotImplementedException();
        }
    }
}
