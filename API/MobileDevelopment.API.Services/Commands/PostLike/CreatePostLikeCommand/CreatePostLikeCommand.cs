using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.PostLikes;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.PostLike.CreatePostLikeCommand
{
    public sealed record CreatePostLikeCommand(CreateEditPostLikeDto Dto) : IRequest<Result<PostLikeDto>>;

    public sealed class CreatePostLikeCommandValidator : AbstractValidator<CreatePostLikeCommand>
    {
        public CreatePostLikeCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.PostId).GreaterThan(0).WithMessage("PostId must be greater than 0.");
            RuleFor(x => x.Dto.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }

    public sealed class CreatePostLikeCommandHandler : IRequestHandler<CreatePostLikeCommand, Result<PostLikeDto>>
    {
        private readonly IPostLikeService _service;

        public CreatePostLikeCommandHandler(IPostLikeService service)
        {
            _service = service;
        }

        public Task<Result<PostLikeDto>> Handle(CreatePostLikeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
