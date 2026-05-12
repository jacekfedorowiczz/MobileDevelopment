using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Users;
using MobileDevelopment.API.Models.Pagination;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Queries.User
{
    public sealed record GetPagedUsersQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<UserDto>>>;

    public sealed class GetPagedUsersQueryValidator : AbstractValidator<GetPagedUsersQuery>
    {
        public GetPagedUsersQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }

    public sealed class GetPagedUsersQueryHandler : IRequestHandler<GetPagedUsersQuery, Result<PagedResult<UserDto>>>
    {
        public Task<Result<PagedResult<UserDto>>> Handle(GetPagedUsersQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}