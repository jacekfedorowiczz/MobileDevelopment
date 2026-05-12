using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Tag
{
    public sealed record GetTagQuery(int Id) : IRequest<Result<TagDto>>;

    public sealed class GetTagQueryValidator : AbstractValidator<GetTagQuery>
    {
        public GetTagQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetTagQueryHandler : IRequestHandler<GetTagQuery, Result<TagDto>>
    {
        public Task<Result<TagDto>> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}