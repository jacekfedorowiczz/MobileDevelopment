using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.Profile
{
    public sealed record GetProfileQuery(int Id) : IRequest<Result<ProfileDto>>;

    public sealed class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
    {
        public GetProfileQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileDto>>
    {
        public Task<Result<ProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}