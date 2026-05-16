using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

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

    public sealed class EditDietDayCommandHandler(IDietDayService dietDayService) : IRequestHandler<EditDietDayCommand, Result<DietDayDto>>
    {
        public Task<Result<DietDayDto>> Handle(EditDietDayCommand request, CancellationToken cancellationToken)
        {
            return dietDayService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        }
    }
}