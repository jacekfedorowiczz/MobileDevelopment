namespace MobileDevelopment.API.Models.DTO.Tags
{
    public sealed record TagDto(
        int Id,
        string Name
    );

    public sealed record CreateEditTagDto(
        string Name
    );
}
