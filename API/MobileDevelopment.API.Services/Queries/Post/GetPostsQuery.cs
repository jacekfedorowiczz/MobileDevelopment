using MediatR;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Post
{
    public sealed record GetPostsQuery() : IRequest<Result<IEnumerable<PostDto>>>;

    public sealed class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, Result<IEnumerable<PostDto>>>
    {
        public Task<Result<IEnumerable<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}