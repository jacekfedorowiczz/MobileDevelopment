using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.MuscleGroup
{
    public sealed record GetMuscleGroupQuery(int Id) : IRequest<Result<MuscleGroupDto>>;

    public sealed class GetMuscleGroupQueryValidator : AbstractValidator<GetMuscleGroupQuery>
    {
        public GetMuscleGroupQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetMuscleGroupQueryHandler(IExerciseService exerciseService) : IRequestHandler<GetMuscleGroupQuery, Result<MuscleGroupDto>>
    {
        public Task<Result<MuscleGroupDto>> Handle(GetMuscleGroupQuery request, CancellationToken cancellationToken)
        {
            return exerciseService.GetMuscleGroupByIdAsync(request.Id, cancellationToken);
        }
    }
}