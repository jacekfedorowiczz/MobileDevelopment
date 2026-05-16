using MobileDevelopment.API.Models.DTO.Dashboard;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<Result<DashboardSummaryDto>> GetSummaryAsync(int userId, CancellationToken ct = default);
    }
}
