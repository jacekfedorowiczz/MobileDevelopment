using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.DietDay
{
    public sealed record RemoveRangeDietDaysCommand(IEnumerable<int> Ids) : IRequest<Result<bool>>, IRemoveRangeCommand;

    public sealed class RemoveRangeDietDaysCommandHandler : IRequestHandler<RemoveRangeDietDaysCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveRangeDietDaysCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}