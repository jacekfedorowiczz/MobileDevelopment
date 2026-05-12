using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Exercises;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Exercise
{
    public sealed record EditExerciseCommand(int Id, CreateEditExerciseDto Dto) : IRequest<Result<ExerciseDto>>;

    public sealed class EditExerciseCommandValidator : AbstractValidator<EditExerciseCommand>
    {
        public EditExerciseCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditExerciseCommandHandler : IRequestHandler<EditExerciseCommand, Result<ExerciseDto>>
    {
        public Task<Result<ExerciseDto>> Handle(EditExerciseCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}