using MediatR;
using MobileDevelopment.API.Models.DTO.Gyms;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Gym
{
    public sealed record EditGymCommand(int Id, CreateEditGymDto Dto) : IRequest<Result<GymDto>>;

    public sealed class EditGymCommandHandler : IRequestHandler<EditGymCommand, Result<GymDto>>
    {
        private readonly IGymService _gymService;

        public EditGymCommandHandler(IGymService gymService)
        {
            _gymService = gymService;
        }

        public async Task<Result<GymDto>> Handle(EditGymCommand request, CancellationToken cancellationToken)
        {
            return await _gymService.EditGymAsync(request.Id, request.Dto);
        }
    }
}
