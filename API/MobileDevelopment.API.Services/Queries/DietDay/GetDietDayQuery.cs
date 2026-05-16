using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.DietDay
{
    public sealed record GetDietDayQuery(int Id) : IRequest<Result<DietDayDto>>;

    public sealed class GetDietDayQueryValidator : AbstractValidator<GetDietDayQuery>
    {
        public GetDietDayQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetDietDayQueryHandler(IDietDayService dietDayService) : IRequestHandler<GetDietDayQuery, Result<DietDayDto>>
    {
        public Task<Result<DietDayDto>> Handle(GetDietDayQuery request, CancellationToken cancellationToken)
        {
            return dietDayService.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}