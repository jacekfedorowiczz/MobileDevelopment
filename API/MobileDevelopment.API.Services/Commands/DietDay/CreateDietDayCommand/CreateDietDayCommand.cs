using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.DietDays;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.DietDay.CreateDietDayCommand
{
    public sealed record CreateDietDayCommand(CreateEditDietDayDto Dto) : IRequest<Result<DietDayDto>>;

    public sealed class CreateDietDayCommandValidator : AbstractValidator<CreateDietDayCommand>
    {
        public CreateDietDayCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.DietId).GreaterThan(0).WithMessage("DietId must be greater than 0.");
            RuleFor(x => x.Dto.Date).NotEmpty().WithMessage("Date cannot be empty.");
        }
    }

    public sealed class CreateDietDayCommandHandler : IRequestHandler<CreateDietDayCommand, Result<DietDayDto>>
    {
        private readonly IDietDayService _service;

        public CreateDietDayCommandHandler(IDietDayService service)
        {
            _service = service;
        }

        public Task<Result<DietDayDto>> Handle(CreateDietDayCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
