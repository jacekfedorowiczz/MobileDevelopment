using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.Meal
{
    public sealed record RemoveMealCommand(int Id) : IRequest<Result>, IRemoveCommand;

    public sealed class RemoveMealCommandHandler(IMealService mealService) : IRequestHandler<RemoveMealCommand, Result>
    {
        public Task<Result> Handle(RemoveMealCommand request, CancellationToken cancellationToken)
        {
            return mealService.DeleteAsync(request.Id, cancellationToken);
        }
    }
}