using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.PostLikes;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.PostLike.GetPostLikeQuery
{
    public sealed record GetPostLikeQuery(int PostId, int Id) : IRequest<Result<PostLikeDto>>;

    public sealed class GetPostLikeQueryValidator : AbstractValidator<GetPostLikeQuery>
    {
        public GetPostLikeQueryValidator()
        {
            RuleFor(x => x.PostId).GreaterThan(0).WithMessage("PostId must be greater than 0.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetPostLikeQueryHandler : IRequestHandler<GetPostLikeQuery, Result<PostLikeDto>>
    {
        private readonly IPostLikeService _service;

        public GetPostLikeQueryHandler(IPostLikeService service)
        {
            _service = service;
        }

        public Task<Result<PostLikeDto>> Handle(GetPostLikeQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
