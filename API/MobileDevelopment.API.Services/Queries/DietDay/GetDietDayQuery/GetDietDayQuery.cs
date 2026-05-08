using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.DietDay.GetDietDayQuery
{
    public sealed record GetDietDayQuery(int DietId, int Id) : IRequest<Result<DietDayDto>>;

    public sealed class GetDietDayQueryValidator : AbstractValidator<GetDietDayQuery>
    {
        public GetDietDayQueryValidator()
        {
            RuleFor(x => x.DietId).GreaterThan(0).WithMessage("DietId must be greater than 0.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetDietDayQueryHandler : IRequestHandler<GetDietDayQuery, Result<DietDayDto>>
    {
        private readonly IDietDayService _service;

        public GetDietDayQueryHandler(IDietDayService service)
        {
            _service = service;
        }

        public Task<Result<DietDayDto>> Handle(GetDietDayQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
