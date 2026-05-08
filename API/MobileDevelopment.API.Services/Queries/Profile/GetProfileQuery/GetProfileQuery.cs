using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.Profile.GetProfileQuery
{
    public sealed record GetProfileQuery(int UserId) : IRequest<Result<ProfileDto>>;

    public sealed class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
    {
        public GetProfileQueryValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }

    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileDto>>
    {
        private readonly IProfileService _service;

        public GetProfileQueryHandler(IProfileService service)
        {
            _service = service;
        }

        public Task<Result<ProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
