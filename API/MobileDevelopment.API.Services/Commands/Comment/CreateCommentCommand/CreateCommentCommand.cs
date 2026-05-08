using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Comments;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Comment.CreateCommentCommand
{
    public sealed record CreateCommentCommand(CreateEditCommentDto Dto) : IRequest<Result<CommentDto>>;

    public sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.PostId).GreaterThan(0).WithMessage("PostId must be greater than 0.");
            RuleFor(x => x.Dto.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Dto.Content).NotEmpty().WithMessage("Content cannot be empty.");
        }
    }

    public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
    {
        private readonly ICommentService _service;

        public CreateCommentCommandHandler(ICommentService service)
        {
            _service = service;
        }

        public Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
