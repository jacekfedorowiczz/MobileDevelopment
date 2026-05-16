using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.DietDay
{
    public sealed record RemoveDietDayCommand(int Id) : IRequest<Result>, IRemoveCommand;

    public sealed class RemoveDietDayCommandHandler(IDietDayService dietDayService) : IRequestHandler<RemoveDietDayCommand, Result>
    {
        public Task<Result> Handle(RemoveDietDayCommand request, CancellationToken cancellationToken)
        {
            return dietDayService.DeleteAsync(request.Id, cancellationToken);
        }
    }
}