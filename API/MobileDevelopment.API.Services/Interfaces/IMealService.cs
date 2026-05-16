using MobileDevelopment.API.Models.DTO.Meals;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IMealService
    {
        Task<Result<MealDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Result<PagedResult<MealDto>>> GetPagedByDietDayIdAsync(int dietDayId, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<Result<IEnumerable<MealDto>>> GetAllByDietDayIdAsync(int dietDayId, CancellationToken ct = default);
        Task<Result<MealDto>> CreateAsync(CreateEditMealDto dto, CancellationToken ct = default);
        Task<Result<MealDto>> UpdateAsync(int id, CreateEditMealDto dto, CancellationToken ct = default);
        Task<Result> DeleteAsync(int id, CancellationToken ct = default);
        Task<Result> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken ct = default);
    }
}
