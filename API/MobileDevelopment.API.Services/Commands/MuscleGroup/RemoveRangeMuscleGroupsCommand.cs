using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.MuscleGroup
{
    public sealed record RemoveRangeMuscleGroupsCommand(IEnumerable<int> Ids) : IRequest<Result<bool>>, IRemoveRangeCommand;

    public sealed class RemoveRangeMuscleGroupsCommandHandler : IRequestHandler<RemoveRangeMuscleGroupsCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveRangeMuscleGroupsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}