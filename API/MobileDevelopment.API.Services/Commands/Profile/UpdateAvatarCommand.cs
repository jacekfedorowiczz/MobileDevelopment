using MediatR;
using Microsoft.AspNetCore.Http;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Profile
{
    public sealed record UpdateAvatarCommand(IFormFile File) : IRequest<Result<string>>;

    public sealed class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, Result<string>>
    {
        private readonly IProfileService _profileService;

        public UpdateAvatarCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public Task<Result<string>> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            return _profileService.UpdateAvatarAsync(request.File, cancellationToken);
        }
    }
}
