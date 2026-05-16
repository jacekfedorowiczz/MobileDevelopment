using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.WorkoutSession
{
    public sealed record GetWorkoutSessionQuery(int Id) : IRequest<Result<WorkoutSessionDto>>;

    public sealed class GetWorkoutSessionQueryValidator : AbstractValidator<GetWorkoutSessionQuery>
    {
        public GetWorkoutSessionQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetWorkoutSessionQueryHandler(IWorkoutSessionService workoutSessionService) : IRequestHandler<GetWorkoutSessionQuery, Result<WorkoutSessionDto>>
    {
        public Task<Result<WorkoutSessionDto>> Handle(GetWorkoutSessionQuery request, CancellationToken cancellationToken)
        {
            return workoutSessionService.GetSessionByIdAsync(request.Id, cancellationToken);
        }
    }
}