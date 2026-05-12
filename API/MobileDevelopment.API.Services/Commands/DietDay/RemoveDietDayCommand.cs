using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.DietDay
{
    public sealed record RemoveDietDayCommand(int Id) : IRequest<Result<bool>>, IRemoveCommand;

    public sealed class RemoveDietDayCommandHandler : IRequestHandler<RemoveDietDayCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(RemoveDietDayCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}