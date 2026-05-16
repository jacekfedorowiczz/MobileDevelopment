using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Comment
{
    public sealed record GetCommentQuery(int Id) : IRequest<Result<CommentDto>>;

    public sealed class GetCommentQueryValidator : AbstractValidator<GetCommentQuery>
    {
        public GetCommentQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, Result<CommentDto>>
    {
        private readonly ICommentService _service;
        private readonly ILogger<GetCommentQueryHandler> _logger;

        public GetCommentQueryHandler(ICommentService service, ILogger<GetCommentQueryHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<Result<CommentDto>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.GetByIdAsync(request.Id, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while fetching comment: {Message}", e.Message);
                return Result<CommentDto>.Failure(e.Message);
            }
        }
    }
}