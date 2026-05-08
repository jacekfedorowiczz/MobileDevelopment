using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Profile.CreateProfileCommand
{
    public sealed record CreateProfileCommand(CreateEditProfileDto Dto) : IRequest<Result<ProfileDto>>;

    public sealed class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
    {
        public CreateProfileCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Dto.Weight).GreaterThan(0).WithMessage("Weight must be greater than 0.");
            RuleFor(x => x.Dto.Height).GreaterThan(0).WithMessage("Height must be greater than 0.");
        }
    }

    public sealed class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Result<ProfileDto>>
    {
        private readonly IProfileService _service;

        public CreateProfileCommandHandler(IProfileService service)
        {
            _service = service;
        }

        public Task<Result<ProfileDto>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
