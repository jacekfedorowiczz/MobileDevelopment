using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Services
{
    internal sealed class WorkoutSessionService : IWorkoutSessionService
    {
        private readonly IWorkoutSessionRepository _repo;

        public WorkoutSessionService(IWorkoutSessionRepository repo)
        {
            _repo = repo;
        }

        public async Task<WorkoutSession> GetSessionById(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
