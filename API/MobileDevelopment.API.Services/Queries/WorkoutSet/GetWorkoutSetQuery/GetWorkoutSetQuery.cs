using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.WorkoutSets;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.WorkoutSet.GetWorkoutSetQuery
{
    public sealed record GetWorkoutSetQuery(int WorkoutSessionId, int Id) : IRequest<Result<WorkoutSetDto>>;

    public sealed class GetWorkoutSetQueryValidator : AbstractValidator<GetWorkoutSetQuery>
    {
        public GetWorkoutSetQueryValidator()
        {
            RuleFor(x => x.WorkoutSessionId).GreaterThan(0).WithMessage("WorkoutSessionId must be greater than 0.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetWorkoutSetQueryHandler : IRequestHandler<GetWorkoutSetQuery, Result<WorkoutSetDto>>
    {
        private readonly IWorkoutSetService _service;

        public GetWorkoutSetQueryHandler(IWorkoutSetService service)
        {
            _service = service;
        }

        public Task<Result<WorkoutSetDto>> Handle(GetWorkoutSetQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
