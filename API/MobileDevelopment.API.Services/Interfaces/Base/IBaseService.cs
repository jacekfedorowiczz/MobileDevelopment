using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Model;

namespace MobileDevelopment.API.Services.Interfaces.Base
{
    public interface IBaseService<TDto, TCreateEditDto>
    {
        Task<Result<TDto>> GetByIdAsync(int id, CancellationToken token = default);
        Task<Result<IEnumerable<TDto>>> GetAllAsync(CancellationToken token = default);
        Task<Result<int>> CreateAsync(TCreateEditDto dto, CancellationToken token = default);
        Task<Result> UpdateAsync(int id, TCreateEditDto dto, CancellationToken token = default);
        Task<Result> DeleteAsync(int id, CancellationToken token = default);
        Task<Result> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken token = default);
        Task<Result<PagedResult<TDto>>> GetPaginatedResultAsync(GetPagedQuery<TDto> query, CancellationToken token);
    }
}
