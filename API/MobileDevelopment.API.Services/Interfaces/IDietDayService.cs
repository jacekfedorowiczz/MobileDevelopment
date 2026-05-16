using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IDietDayService
    {
        Task<Result<DietDayDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Result<PagedResult<DietDayDto>>> GetPagedByDietIdAsync(int dietId, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<Result<IEnumerable<DietDayDto>>> GetAllByDietIdAsync(int dietId, CancellationToken ct = default);
        Task<Result<DietDayDto>> CreateAsync(CreateEditDietDayDto dto, CancellationToken ct = default);
        Task<Result<DietDayDto>> CreateWithMealsAsync(CreateDietDayWithMealsDto dto, CancellationToken ct = default);
        Task<Result<DietDayDto>> UpdateAsync(int id, CreateEditDietDayDto dto, CancellationToken ct = default);
        Task<Result> DeleteAsync(int id, CancellationToken ct = default);
        Task<Result> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken ct = default);
    }
}
