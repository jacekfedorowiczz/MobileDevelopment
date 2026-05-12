using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Comment
{
    public sealed record RemoveRangeCommentsCommand(IEnumerable<int> Ids) : IRequest<Result<bool>>, IRemoveRangeCommand;

    public sealed class RemoveRangeCommentsCommandHandler : IRequestHandler<RemoveRangeCommentsCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveRangeCommentsCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}