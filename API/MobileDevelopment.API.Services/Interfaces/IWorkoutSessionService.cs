using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IWorkoutSessionService
    {
        Task<WorkoutSession> GetSessionById(int id, CancellationToken cancellationToken = default);
    }
}
