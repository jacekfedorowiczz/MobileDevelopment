using MediatR;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Comment
{
    public sealed record GetAllCommentsQuery() : IRequest<Result<IEnumerable<CommentDto>>>;

    public sealed class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentsQuery, Result<IEnumerable<CommentDto>>>
    {
        private readonly ICommentService _service;
        private readonly ILogger<GetCommentQueryHandler> _logger;

        public GetAllCommentsQueryHandler(ICommentService service, ILogger<GetCommentQueryHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CommentDto>>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.GetAllAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while fetching comments: {Message}", e.Message);
                return Result<IEnumerable<CommentDto>>.Failure(e.Message);
            }
        }
    }
}