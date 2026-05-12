using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.DietDay
{
    public sealed record CreateDietDayCommand(CreateEditDietDayDto Dto) : IRequest<Result<DietDayDto>>;

    public sealed class CreateDietDayCommandValidator : AbstractValidator<CreateDietDayCommand>
    {
        public CreateDietDayCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreateDietDayCommandHandler : IRequestHandler<CreateDietDayCommand, Result<DietDayDto>>
    {
        public Task<Result<DietDayDto>> Handle(CreateDietDayCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}