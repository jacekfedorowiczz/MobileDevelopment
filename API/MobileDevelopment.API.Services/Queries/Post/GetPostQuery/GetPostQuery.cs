using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Posts;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Post.GetPostQuery
{
    public sealed record GetPostQuery(int UserId, int Id) : IRequest<Result<PostDto>>;

    public sealed class GetPostQueryValidator : AbstractValidator<GetPostQuery>
    {
        public GetPostQueryValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetPostQueryHandler : IRequestHandler<GetPostQuery, Result<PostDto>>
    {
        private readonly IPostService _service;

        public GetPostQueryHandler(IPostService service)
        {
            _service = service;
        }

        public Task<Result<PostDto>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
