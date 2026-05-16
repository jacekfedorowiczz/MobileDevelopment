using MediatR;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Profile
{
    public sealed class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, Result<MyProfileDto>>
    {
        private readonly IProfileService _profileService;

        public GetMyProfileQueryHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public Task<Result<MyProfileDto>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            return _profileService.GetMyProfileAsync(cancellationToken);
        }
    }
}
