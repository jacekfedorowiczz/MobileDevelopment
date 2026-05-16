using MediatR;
using MobileDevelopment.API.Models.DTO.Achievements;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Achievement
{
    public sealed record GetMyAchievementsQuery : IRequest<Result<IEnumerable<ProfileAchievementDto>>>;

    public sealed class GetMyAchievementsQueryHandler(IAchievementService achievementService)
        : IRequestHandler<GetMyAchievementsQuery, Result<IEnumerable<ProfileAchievementDto>>>
    {
        public Task<Result<IEnumerable<ProfileAchievementDto>>> Handle(GetMyAchievementsQuery request, CancellationToken cancellationToken)
        {
            return achievementService.GetMyAchievementsAsync(cancellationToken);
        }
    }
}
