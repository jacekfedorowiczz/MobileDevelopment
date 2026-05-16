using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Persistence.Repositories.Base;

namespace MobileDevelopment.API.Persistence.Repositories
{
    public sealed class MealRepository(SystemContext context)
        : Repository<Meal>(context), IMealRepository
    {
    }
}
