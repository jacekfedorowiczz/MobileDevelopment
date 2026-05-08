using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;

namespace MobileDevelopment.API.Persistence.Repositories
{
    public class ProfileRepository(SystemContext context)
        : Repository<Profile>(context), IProfileRepository
    {
    }
}
