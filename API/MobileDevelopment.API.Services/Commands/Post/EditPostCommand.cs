using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Commands.Post
{
    public sealed record EditPostCommand(int Id, CreateEditPostDto Dto) : IRequest<Result<PostDto>>;

    public sealed class EditPostCommandValidator : AbstractValidator<EditPostCommand>
    {
        public EditPostCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditPostCommandHandler : IRequestHandler<EditPostCommand, Result<PostDto>>
    {
        public Task<Result<PostDto>> Handle(EditPostCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}