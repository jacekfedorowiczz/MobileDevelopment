using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.PostLikes;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Services.Commands.PostLike
{
    public sealed record CreatePostLikeCommand(CreateEditPostLikeDto Dto) : IRequest<Result<PostLikeDto>>;

    public sealed class CreatePostLikeCommandValidator : AbstractValidator<CreatePostLikeCommand>
    {
        public CreatePostLikeCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreatePostLikeCommandHandler : IRequestHandler<CreatePostLikeCommand, Result<PostLikeDto>>
    {
        public Task<Result<PostLikeDto>> Handle(CreatePostLikeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}