using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Gym
{
    public sealed record RemoveRangeGymsCommand(IEnumerable<int> Ids) : IRequest<Result>;

    public sealed class RemoveRangeGymsCommandHandler : IRequestHandler<RemoveRangeGymsCommand, Result>
    {
        private readonly IGymService _gymService;

        public RemoveRangeGymsCommandHandler(IGymService gymService)
        {
            _gymService = gymService;
        }

        public async Task<Result> Handle(RemoveRangeGymsCommand request, CancellationToken cancellationToken)
        {
            return await _gymService.RemoveRangeGymsAsync(request.Ids);
        }
    }
}
