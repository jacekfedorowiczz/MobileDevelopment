using FluentValidation;
using MediatR;
using MobileDevelopment.API.Models.DTO.Tags;
using MobileDevelopment.API.Models.Wrappers;
using MobileDevelopment.API.Services.Interfaces;

namespace MobileDevelopment.API.Services.Commands.Tag.CreateTagCommand
{
    public sealed record CreateTagCommand(CreateEditTagDto Dto) : IRequest<Result<TagDto>>;

    public sealed class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Dto cannot be null.");
            RuleFor(x => x.Dto.Name).NotEmpty().WithMessage("Name cannot be empty.");
        }
    }

    public sealed class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result<TagDto>>
    {
        private readonly ITagService _service;

        public CreateTagCommandHandler(ITagService service)
        {
            _service = service;
        }

        public Task<Result<TagDto>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
