using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Diets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Diet.GetDietQuery
{
    public sealed record GetDietQuery(int UserId, int Id) : IRequest<Result<DietDto>>;

    public sealed class GetDietQueryValidator : AbstractValidator<GetDietQuery>
    {
        public GetDietQueryValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetDietQueryHandler : IRequestHandler<GetDietQuery, Result<DietDto>>
    {
        private readonly IDietService _service;

        public GetDietQueryHandler(IDietService service)
        {
            _service = service;
        }

        public Task<Result<DietDto>> Handle(GetDietQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
