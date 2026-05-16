using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Persistence.Repositories.Base;

namespace MobileDevelopment.API.Persistence.Repositories
{
    public sealed class RefreshTokenRepository(SystemContext context) : Repository<RefreshToken>(context), IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RefreshToken>()
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
        }
    }
}
