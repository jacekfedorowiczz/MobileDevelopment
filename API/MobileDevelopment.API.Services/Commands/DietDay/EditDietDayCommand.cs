using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.DietDay
{
    public sealed record EditDietDayCommand(int Id, CreateEditDietDayDto Dto) : IRequest<Result<DietDayDto>>;

    public sealed class EditDietDayCommandValidator : AbstractValidator<EditDietDayCommand>
    {
        public EditDietDayCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditDietDayCommandHandler : IRequestHandler<EditDietDayCommand, Result<DietDayDto>>
    {
        public Task<Result<DietDayDto>> Handle(EditDietDayCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}