using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Model;

namespace MobileDevelopment.API.Services.Queries.Comment
{
    public sealed record GetPagedCommentsQuery(GetPagedQuery<CommentDto> Query) : IRequest<Result<PagedResult<CommentDto>>>;

    public sealed class GetPagedCommentsQueryValidator : AbstractValidator<GetPagedCommentsQuery>
    {
        public GetPagedCommentsQueryValidator()
        {
            RuleFor(x => x.Query.PageIndex).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.Query.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedCommentsQueryHandler : IRequestHandler<GetPagedCommentsQuery, Result<PagedResult<CommentDto>>>
    {
        private readonly ICommentService _service;
        private readonly ILogger<GetCommentQueryHandler> _logger;

        public GetPagedCommentsQueryHandler(ICommentService service, ILogger<GetCommentQueryHandler> logger)
        {
            _service = service;
            _logger = logger;
        }


        public async Task<Result<PagedResult<CommentDto>>> Handle(GetPagedCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.GetPaginatedResultAsync(request.Query, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while fetching paged comments: {Message}", e.Message);
                return Result<PagedResult<CommentDto>>.Failure(e.Message);
            }
        }
    }
}