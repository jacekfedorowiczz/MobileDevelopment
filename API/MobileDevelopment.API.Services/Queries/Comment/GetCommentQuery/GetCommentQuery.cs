using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Comment.GetCommentQuery
{
    public sealed record GetCommentQuery(int PostId, int Id) : IRequest<Result<CommentDto>>;

    public sealed class GetCommentQueryValidator : AbstractValidator<GetCommentQuery>
    {
        public GetCommentQueryValidator()
        {
            RuleFor(x => x.PostId).GreaterThan(0).WithMessage("PostId must be greater than 0.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, Result<CommentDto>>
    {
        private readonly ICommentService _service;

        public GetCommentQueryHandler(ICommentService service)
        {
            _service = service;
        }

        public Task<Result<CommentDto>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
