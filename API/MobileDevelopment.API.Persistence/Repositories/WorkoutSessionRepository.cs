using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Persistence.Repositories.Base;

namespace MobileDevelopment.API.Persistence.Repositories
{
    internal sealed class WorkoutSessionRepository : Repository<WorkoutSession>, IWorkoutSessionRepository
    {
        public WorkoutSessionRepository(SystemContext context) : base(context)
        {
        }
    }
}
