using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.PostLikes;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.PostLike
{
    public sealed record GetPostLikeQuery(int Id) : IRequest<Result<PostLikeDto>>;

    public sealed class GetPostLikeQueryValidator : AbstractValidator<GetPostLikeQuery>
    {
        public GetPostLikeQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetPostLikeQueryHandler : IRequestHandler<GetPostLikeQuery, Result<PostLikeDto>>
    {
        public Task<Result<PostLikeDto>> Handle(GetPostLikeQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}