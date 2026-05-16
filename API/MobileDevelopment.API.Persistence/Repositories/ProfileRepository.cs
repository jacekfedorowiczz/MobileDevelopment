using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MobileDevelopment.API.Persistence.Repositories
{
    public sealed class ProfileRepository(SystemContext context)
        : Repository<Profile>(context), IProfileRepository
    {
        public Task<Profile?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return _context.Profiles
                .FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken);
        }

        public Task<Profile?> GetByIdWithUserAsync(int id, CancellationToken cancellationToken = default)
        {
            return _context.Profiles
                .Include(profile => profile.User)
                .FirstOrDefaultAsync(profile => profile.Id == id, cancellationToken);
        }

        public Task<Profile?> GetByUserIdWithUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            return _context.Profiles
                .Include(profile => profile.User)
                .FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken);
        }

        public Task<Profile?> GetByUserIdWithUserAndAchievementsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return _context.Profiles
                .Include(profile => profile.User)
                .Include(profile => profile.ProfileAchievements)
                .FirstOrDefaultAsync(profile => profile.UserId == userId, cancellationToken);
        }
    }
}
