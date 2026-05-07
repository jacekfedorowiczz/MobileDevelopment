using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Persistence.Interfaces;

namespace MobileDevelopment.API.Persistence.Repositories
{
    internal sealed class WorkoutSessionRepository : IWorkoutSessionRepository
    {
        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<WorkoutSession> CreateAsync(WorkoutSession entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<WorkoutSession>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<WorkoutSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<WorkoutSession>> GetPagedDynamicAsync(string? searchValue, string? sortColumn, string? sortDirection, int pageIndex, int pageSize, Func<IQueryable<WorkoutSession>, IQueryable<WorkoutSession>>? include = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IQueryable<WorkoutSession> GetQueryable()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(WorkoutSession entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
