using MediatR;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Tag
{
    public sealed record GetTagsQuery() : IRequest<Result<IEnumerable<TagDto>>>;

    public sealed class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, Result<IEnumerable<TagDto>>>
    {
        public Task<Result<IEnumerable<TagDto>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}