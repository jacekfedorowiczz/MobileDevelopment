using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;
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

    public sealed class GetWorkoutSetQueryHandler : IRequestHandler<GetWorkoutSetQuery, Result<WorkoutSetDto>>
    {
        public Task<Result<WorkoutSetDto>> Handle(GetWorkoutSetQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}