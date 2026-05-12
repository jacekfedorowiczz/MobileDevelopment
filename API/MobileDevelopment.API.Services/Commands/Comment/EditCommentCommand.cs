using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Comment
{
    public sealed record EditCommentCommand(int Id, CreateEditCommentDto Dto) : IRequest<Result<CommentDto>>;

    public sealed class EditCommentCommandValidator : AbstractValidator<EditCommentCommand>
    {
        public EditCommentCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, Result<CommentDto>>
    {
        public Task<Result<CommentDto>> Handle(EditCommentCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}