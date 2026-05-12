using MediatR;
using MobileDevelopment.API.Models.DTO.PostLikes;
using MobileDevelopment.API.Models.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.PostLike
{
    public sealed record GetPostLikesQuery() : IRequest<Result<IEnumerable<PostLikeDto>>>;

    public sealed class GetPostLikesQueryHandler : IRequestHandler<GetPostLikesQuery, Result<IEnumerable<PostLikeDto>>>
    {
        public Task<Result<IEnumerable<PostLikeDto>>> Handle(GetPostLikesQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}