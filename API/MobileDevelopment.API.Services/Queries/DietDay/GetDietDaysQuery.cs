using MediatR;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.DietDay
{
    public sealed record GetDietDaysQuery(int DietId) : IRequest<Result<IEnumerable<DietDayDto>>>;

    public sealed class GetDietDaysQueryHandler(IDietDayService dietDayService) : IRequestHandler<GetDietDaysQuery, Result<IEnumerable<DietDayDto>>>
    {
        public Task<Result<IEnumerable<DietDayDto>>> Handle(GetDietDaysQuery request, CancellationToken cancellationToken)
        {
            return dietDayService.GetAllByDietIdAsync(request.DietId, cancellationToken);
        }
    }
}