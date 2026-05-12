using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.User
{
    public sealed record GetUserQuery(int Id) : IRequest<Result<UserDto>>;

    public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }

    public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
    {
        public Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}