using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Gym
{
    public sealed record RemoveGymCommand(int Id) : IRequest<Result>;

    public sealed class RemoveGymCommandHandler : IRequestHandler<RemoveGymCommand, Result>
    {
        private readonly IGymService _gymService;

        public RemoveGymCommandHandler(IGymService gymService)
        {
            _gymService = gymService;
        }

        public async Task<Result> Handle(RemoveGymCommand request, CancellationToken cancellationToken)
        {
            return await _gymService.RemoveGymAsync(request.Id);
        }
    }
}
