using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Models.Pagination;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IGymService
    {
        Task<Result<GymDto>> GetGymByIdAsync(int id);
        Task<Result<IEnumerable<GymDto>>> GetAllGymsAsync(string? search = null);
        Task<Result<PagedResult<GymDto>>> GetPagedGymsAsync(int pageNumber, int pageSize);
        Task<Result<GymDto>> CreateGymAsync(CreateEditGymDto dto);
        Task<Result<GymDto>> EditGymAsync(int id, CreateEditGymDto dto);
        Task<Result> RemoveGymAsync(int id);
        Task<Result> RemoveRangeGymsAsync(IEnumerable<int> ids);
    }
}
