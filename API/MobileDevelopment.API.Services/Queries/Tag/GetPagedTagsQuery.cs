using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Tag
{
    public sealed record GetPagedTagsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<TagDto>>>;

    public sealed class GetPagedTagsQueryValidator : AbstractValidator<GetPagedTagsQuery>
    {
        public GetPagedTagsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedTagsQueryHandler : IRequestHandler<GetPagedTagsQuery, Result<PagedResult<TagDto>>>
    {
        public Task<Result<PagedResult<TagDto>>> Handle(GetPagedTagsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}