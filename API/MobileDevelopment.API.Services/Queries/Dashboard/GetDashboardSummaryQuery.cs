using MediatR;
using MobileDevelopment.API.Models.DTO.Dashboard;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Persistence.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Dashboard
{
    public sealed record GetDashboardSummaryQuery(string UserId) : IRequest<Result<DashboardSummaryDto>>;

    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, Result<DashboardSummaryDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetDashboardSummaryQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<DashboardSummaryDto>> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(int.Parse(request.UserId), cancellationToken);
            if (user is null)
            {
                return Result<DashboardSummaryDto>.Failure("Nie znaleziono użytkownika.");
            }

            // TODO: pobrać dane z bazy


            var dto = new DashboardSummaryDto
            {
                FirstName = user.FirstName,
                CaloriesBurned = 2340,
                TotalSets = 12,
                TotalWorkouts = 24,
                WeeklyActivity = new List<ActivityDayDto>
                {
                    new ActivityDayDto { Day = "Pon", Minutes = 90 },
                    new ActivityDayDto { Day = "Wt", Minutes = 0 },
                    new ActivityDayDto { Day = "Śr", Minutes = 120 },
                    new ActivityDayDto { Day = "Czw", Minutes = 60 },
                    new ActivityDayDto { Day = "Pt", Minutes = 90 },
                    new ActivityDayDto { Day = "Sob", Minutes = 0 },
                    new ActivityDayDto { Day = "Ndz", Minutes = 150 }
                },
                RecentWorkouts = new List<RecentWorkoutDto>
                {
                    new RecentWorkoutDto { Name = "Push (Klatka)", ExercisesCount = 8, Duration = "45 min", Date = "12 kwi" },
                    new RecentWorkoutDto { Name = "Pull (Plecy)", ExercisesCount = 7, Duration = "52 min", Date = "10 kwi" },
                    new RecentWorkoutDto { Name = "FBW", ExercisesCount = 9, Duration = "58 min", Date = "8 kwi" }
                }
            };

            return Result<DashboardSummaryDto>.Success(dto);
        }
    }
}
