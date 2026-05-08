using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;

namespace MobileDevelopment.API.Persistence.Repositories
{
    public class ExerciseRepository(SystemContext context)
        : Repository<Exercise>(context), IExerciseRepository
    {
    }
}
