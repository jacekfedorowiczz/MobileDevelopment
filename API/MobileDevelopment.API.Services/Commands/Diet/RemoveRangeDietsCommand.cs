using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.Diet
{
    public sealed record RemoveRangeDietsCommand(IEnumerable<int> Ids) : IRequest<Result>, IRemoveRangeCommand;

    public sealed class RemoveRangeDietsCommandHandler(IDietService dietService) : IRequestHandler<RemoveRangeDietsCommand, Result>
    {
        public Task<Result> Handle(RemoveRangeDietsCommand request, CancellationToken cancellationToken)
        {
            return dietService.DeleteRangeAsync(request.Ids, cancellationToken);
        }
    }
}