using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Post
{
    public sealed record GetPostQuery(int Id) : IRequest<Result<PostDto>>;

    public sealed class GetPostQueryValidator : AbstractValidator<GetPostQuery>
    {
        public GetPostQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetPostQueryHandler : IRequestHandler<GetPostQuery, Result<PostDto>>
    {
        public Task<Result<PostDto>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}