using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Profiles;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Profile
{
    public sealed record EditProfileCommand(int Id, CreateEditProfileDto Dto) : IRequest<Result<ProfileDto>>;

    public sealed class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
    {
        public EditProfileCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, Result<ProfileDto>>
    {
        public Task<Result<ProfileDto>> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}