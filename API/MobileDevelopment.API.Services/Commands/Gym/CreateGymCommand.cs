using MediatR;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Gym
{
    public sealed record CreateGymCommand(CreateEditGymDto Dto) : IRequest<Result<GymDto>>;

    public sealed class CreateGymCommandHandler : IRequestHandler<CreateGymCommand, Result<GymDto>>
    {
        private readonly IGymService _gymService;

        public CreateGymCommandHandler(IGymService gymService)
        {
            _gymService = gymService;
        }

        public async Task<Result<GymDto>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
        {
            return await _gymService.CreateGymAsync(request.Dto);
        }
    }
}
