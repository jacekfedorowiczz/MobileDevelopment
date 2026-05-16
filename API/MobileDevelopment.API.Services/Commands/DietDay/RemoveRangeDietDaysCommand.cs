using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.DietDay
{
    public sealed record RemoveRangeDietDaysCommand(IEnumerable<int> Ids) : IRequest<Result>, IRemoveRangeCommand;

    public sealed class RemoveRangeDietDaysCommandHandler(IDietDayService dietDayService) : IRequestHandler<RemoveRangeDietDaysCommand, Result>
    {
        public Task<Result> Handle(RemoveRangeDietDaysCommand request, CancellationToken cancellationToken)
        {
            return dietDayService.DeleteRangeAsync(request.Ids, cancellationToken);
        }
    }
}