using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSessions;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.WorkoutSession.GetWorkoutSessionQuery
{
    public sealed record GetWorkoutSessionQuery(int UserId, int Id) : IRequest<Result<WorkoutSessionDto>>;

    public sealed class GetWorkoutSessionQueryValidator : AbstractValidator<GetWorkoutSessionQuery>
    {
        public GetWorkoutSessionQueryValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetWorkoutSessionQueryHandler : IRequestHandler<GetWorkoutSessionQuery, Result<WorkoutSessionDto>>
    {
        private readonly IWorkoutSessionService _service;

        public GetWorkoutSessionQueryHandler(IWorkoutSessionService service)
        {
            _service = service;
        }

        public Task<Result<WorkoutSessionDto>> Handle(GetWorkoutSessionQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
