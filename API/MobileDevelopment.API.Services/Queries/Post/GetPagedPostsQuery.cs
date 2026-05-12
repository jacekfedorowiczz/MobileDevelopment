using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Post
{
    public sealed record GetPagedPostsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<PostDto>>>;

    public sealed class GetPagedPostsQueryValidator : AbstractValidator<GetPagedPostsQuery>
    {
        public GetPagedPostsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedPostsQueryHandler : IRequestHandler<GetPagedPostsQuery, Result<PagedResult<PostDto>>>
    {
        public Task<Result<PagedResult<PostDto>>> Handle(GetPagedPostsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}