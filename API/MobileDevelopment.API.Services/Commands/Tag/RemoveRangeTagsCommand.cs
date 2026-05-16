using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.Tag
{
    public sealed record RemoveRangeTagsCommand(IEnumerable<int> Ids) : IRequest<Result<bool>>, IRemoveRangeCommand;

    public sealed class RemoveRangeTagsCommandHandler : IRequestHandler<RemoveRangeTagsCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveRangeTagsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}