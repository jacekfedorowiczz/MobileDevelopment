using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Tag
{
    public sealed record CreateTagCommand(CreateEditTagDto Dto) : IRequest<Result<TagDto>>;

    public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result<TagDto>>
    {
        public Task<Result<TagDto>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}