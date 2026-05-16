using MediatR;
using MobileDevelopment.API.Models.DTO.Achievements;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Achievement
{
    public sealed record GetAllAchievementsQuery : IRequest<Result<IEnumerable<AchievementDto>>>;

    public sealed class GetAllAchievementsQueryHandler(IAchievementService achievementService)
        : IRequestHandler<GetAllAchievementsQuery, Result<IEnumerable<AchievementDto>>>
    {
        public Task<Result<IEnumerable<AchievementDto>>> Handle(GetAllAchievementsQuery request, CancellationToken cancellationToken)
        {
            return achievementService.GetAllAchievementsAsync(cancellationToken);
        }
    }
}
