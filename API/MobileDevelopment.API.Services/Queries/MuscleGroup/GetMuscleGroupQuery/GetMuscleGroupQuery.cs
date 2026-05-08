using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.MuscleGroups;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.MuscleGroup.GetMuscleGroupQuery
{
    public sealed record GetMuscleGroupQuery(int Id) : IRequest<Result<MuscleGroupDto>>;

    public sealed class GetMuscleGroupQueryValidator : AbstractValidator<GetMuscleGroupQuery>
    {
        public GetMuscleGroupQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetMuscleGroupQueryHandler : IRequestHandler<GetMuscleGroupQuery, Result<MuscleGroupDto>>
    {
        private readonly IMuscleGroupService _service;

        public GetMuscleGroupQueryHandler(IMuscleGroupService service)
        {
            _service = service;
        }

        public Task<Result<MuscleGroupDto>> Handle(GetMuscleGroupQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
