using MediatR;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Profile
{
    public sealed record UpdateDietAssumptionsCommand(CreateEditProfileDto Dto) : IRequest<Result>;

    public sealed class UpdateDietAssumptionsCommandHandler : IRequestHandler<UpdateDietAssumptionsCommand, Result>
    {
        private readonly IProfileService _profileService;

        public UpdateDietAssumptionsCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public Task<Result> Handle(UpdateDietAssumptionsCommand request, CancellationToken cancellationToken)
        {
            return _profileService.UpdateDietAssumptionsAsync(request.Dto, cancellationToken);
        }
    }
}
