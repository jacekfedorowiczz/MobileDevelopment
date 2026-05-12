using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace MobileDevelopment.API.Services.Commands.Tag
{
    public sealed record EditTagCommand(int Id, CreateEditTagDto Dto) : IRequest<Result<TagDto>>;

    public sealed class EditTagCommandValidator : AbstractValidator<EditTagCommand>
    {
        public EditTagCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
        }
    }

    public sealed class EditTagCommandHandler : IRequestHandler<EditTagCommand, Result<TagDto>>
    {
        public Task<Result<TagDto>> Handle(EditTagCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}