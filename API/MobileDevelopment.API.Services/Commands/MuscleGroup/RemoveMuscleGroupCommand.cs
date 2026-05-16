using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.MuscleGroup
{
    public sealed record RemoveMuscleGroupCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveMuscleGroupCommandHandler : IRequestHandler<RemoveMuscleGroupCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveMuscleGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}