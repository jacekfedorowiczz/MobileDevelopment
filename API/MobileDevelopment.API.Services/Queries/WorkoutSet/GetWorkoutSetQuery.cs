using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.WorkoutSet
{
    public sealed record GetWorkoutSetQuery(int Id) : IRequest<Result<WorkoutSetDto>>;

    public sealed class GetWorkoutSetQueryValidator : AbstractValidator<GetWorkoutSetQuery>
    {
        public GetWorkoutSetQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetWorkoutSetQueryHandler(IWorkoutSetService workoutSetService) : IRequestHandler<GetWorkoutSetQuery, Result<WorkoutSetDto>>
    {
        public Task<Result<WorkoutSetDto>> Handle(GetWorkoutSetQuery request, CancellationToken cancellationToken)
        {
            return workoutSetService.GetSetByIdAsync(request.Id, cancellationToken);
        }
    }
}