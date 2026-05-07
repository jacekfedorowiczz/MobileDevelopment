using MobileDevelopment.API.Domain.Base;
using MobileDevelopment.API.Models.Pagination;

namespace MobileDevelopment.API.Persistence.Interfaces
{
    public interface IBaseEntityRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetQueryable();
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<PagedResult<T>> GetPagedDynamicAsync(
            string? searchValue,
            string? sortColumn,
            string? sortDirection,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            CancellationToken cancellationToken = default);
    }
}
