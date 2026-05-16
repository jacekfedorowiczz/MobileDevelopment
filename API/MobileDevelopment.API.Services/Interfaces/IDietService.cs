using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IDietService
    {
        Task<Result<DietDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Result<DietSummaryDto>> GetSummaryForUserAsync(int userId, CancellationToken ct = default);
        Task<Result<PagedResult<DietDto>>> GetPagedDietsForCurrentUserAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<Result<IEnumerable<DietDto>>> GetAllForCurrentUserAsync(CancellationToken ct = default);
        Task<Result<DietDto>> CreateAsync(CreateEditDietDto dto, CancellationToken ct = default);
        Task<Result<DietDto>> CreateWithDaysAsync(CreateDietWithDaysDto dto, CancellationToken ct = default);
        Task<Result<DietDto>> UpdateAsync(int id, CreateEditDietDto dto, CancellationToken ct = default);
        Task<Result> DeleteAsync(int id, CancellationToken ct = default);
        Task<Result> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken ct = default);
    }
}
