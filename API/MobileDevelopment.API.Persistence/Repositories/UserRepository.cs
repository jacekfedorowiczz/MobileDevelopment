using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Persistence.Repositories.Base;


namespace MobileDevelopment.API.Persistence.Repositories
{
    public sealed class UserRepository(SystemContext context) : Repository<User>(context), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
    }
}
