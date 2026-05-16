using MediatR;
using MobileDevelopment.API.Models.DTO.Dashboard;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Dashboard
{
    public sealed record GetDashboardSummaryQuery(string UserId) : IRequest<Result<DashboardSummaryDto>>;

    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, Result<DashboardSummaryDto>>
    {
        private readonly IDashboardService _dashboardService;

        public GetDashboardSummaryQueryHandler(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<Result<DashboardSummaryDto>> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            if (!int.TryParse(request.UserId, out var userId))
            {
                return Result<DashboardSummaryDto>.Failure("Invalid user id.");
            }

            return await _dashboardService.GetSummaryAsync(userId, cancellationToken);
        }
    }
}
