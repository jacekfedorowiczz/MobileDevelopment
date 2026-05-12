using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Commands.Comment
{
    public sealed record CreateCommentCommand(CreateEditCommentDto Dto) : IRequest<Result<CommentDto>>;

    public sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
    {
        public Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}