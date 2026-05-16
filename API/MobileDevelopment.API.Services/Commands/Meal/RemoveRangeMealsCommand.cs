using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.Meal
{
    public sealed record RemoveRangeMealsCommand(IEnumerable<int> Ids) : IRequest<Result>, IRemoveRangeCommand;

    public sealed class RemoveRangeMealsCommandHandler(IMealService mealService) : IRequestHandler<RemoveRangeMealsCommand, Result>
    {
        public Task<Result> Handle(RemoveRangeMealsCommand request, CancellationToken cancellationToken)
        {
            return mealService.DeleteRangeAsync(request.Ids, cancellationToken);
        }
    }
}