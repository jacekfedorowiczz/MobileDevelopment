using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Queries.User.GetUserQuery
{
    public sealed record GetUserQuery(int Id) : IRequest<Result<UserDto>>;
    
    public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id cannot be null or empty");
        }
    }

    public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
    {
        private readonly IUserService _service;

        public GetUserQueryHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
