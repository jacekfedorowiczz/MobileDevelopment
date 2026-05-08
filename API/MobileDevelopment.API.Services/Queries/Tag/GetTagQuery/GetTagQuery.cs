using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Tag.GetTagQuery
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
        private readonly ITagService _service;

        public GetTagQueryHandler(ITagService service)
        {
            _service = service;
        }

        public Task<Result<TagDto>> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
