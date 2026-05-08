using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Post.CreatePostCommand
{
    public sealed record CreatePostCommand(CreateEditPostDto Dto) : IRequest<Result<PostDto>>;

    public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Dto.Title).NotEmpty().WithMessage("Title cannot be empty.");
            RuleFor(x => x.Dto.Content).NotEmpty().WithMessage("Content cannot be empty.");
        }
    }

    public sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result<PostDto>>
    {
        private readonly IPostService _service;

        public CreatePostCommandHandler(IPostService service)
        {
            _service = service;
        }

        public Task<Result<PostDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
