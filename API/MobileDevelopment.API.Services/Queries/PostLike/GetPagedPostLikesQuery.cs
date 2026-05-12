using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.PostLikes;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.PostLike
{
    public sealed record GetPagedPostLikesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<PostLikeDto>>>;

    public sealed class GetPagedPostLikesQueryValidator : AbstractValidator<GetPagedPostLikesQuery>
    {
        public GetPagedPostLikesQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedPostLikesQueryHandler : IRequestHandler<GetPagedPostLikesQuery, Result<PagedResult<PostLikeDto>>>
    {
        public Task<Result<PagedResult<PostLikeDto>>> Handle(GetPagedPostLikesQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}