using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Commands.Post
{
    public sealed record CreatePostCommand(CreateEditPostDto Dto) : IRequest<Result<PostDto>>;

    public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result<PostDto>>
    {
        public Task<Result<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}