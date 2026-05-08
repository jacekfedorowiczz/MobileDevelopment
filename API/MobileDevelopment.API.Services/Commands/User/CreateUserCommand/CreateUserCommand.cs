using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.User.CreateUserCommand
{
    public sealed record CreateUserCommand(CreateEditUserDto Dto) : IRequest<Result<UserDto>>;

    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.Login).NotEmpty().WithMessage("Login cannot be empty.");
            RuleFor(x => x.Dto.FirstName).NotEmpty().WithMessage("FirstName cannot be empty.");
            RuleFor(x => x.Dto.LastName).NotEmpty().WithMessage("LastName cannot be empty.");
            RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.Dto.MobilePhone).NotEmpty().WithMessage("MobilePhone cannot be empty.");
            RuleFor(x => x.Dto.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }

    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly IUserService _service;

        public CreateUserCommandHandler(IUserService service)
        {
            _service = service;
        }

        public Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
