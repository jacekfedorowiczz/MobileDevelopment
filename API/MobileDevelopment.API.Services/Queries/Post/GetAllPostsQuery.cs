using MediatR;
using FluentValidation;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Post
{
    public sealed record GetAllPostsQuery(int PageNumber, int PageSize) : IRequest<Result<PagedResult<PostDto>>>;

    public sealed class GetAllPostsQueryValidator : AbstractValidator<GetAllPostsQuery>
    {
        public GetAllPostsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
        }
    }

    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, Result<PagedResult<PostDto>>>
    {
        private readonly IPostService _postService;

        public GetAllPostsQueryHandler(IPostService postService)
        {
            _postService = postService;
        }

        public Task<Result<PagedResult<PostDto>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            return _postService.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
