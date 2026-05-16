using MediatR;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Interfaces.Commands;

namespace MobileDevelopment.API.Services.Commands.Diet
{
    public sealed record RemoveDietCommand(int Id) : IRequest<Result>, IRemoveCommand;

    public sealed class RemoveDietCommandHandler(IDietService dietService) : IRequestHandler<RemoveDietCommand, Result>
    {
        public Task<Result> Handle(RemoveDietCommand request, CancellationToken cancellationToken)
        {
            return dietService.DeleteAsync(request.Id, cancellationToken);
        }
    }
}