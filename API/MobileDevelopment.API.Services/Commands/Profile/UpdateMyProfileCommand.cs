using MediatR;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Profile
{
    public sealed record UpdateMyProfileCommand(CreateEditProfileDto Dto) : IRequest<Result>;

    public sealed class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, Result>
    {
        private readonly IProfileService _profileService;

        public UpdateMyProfileCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public Task<Result> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
        {
            return _profileService.UpdateMyProfileAsync(request.Dto, cancellationToken);
        }
    }
}
