namespace MobileDevelopment.API.Models.DTO.Diets
{
    public sealed record DietDto(
        int Id,
        int UserId,
        string Name,
        string? Description,
        DateTime StartDate,
        DateTime? EndDate,
        bool IsActive
    );

    public sealed record CreateEditDietDto(
        int UserId,
        string Name,
        string? Description,
        DateTime StartDate,
        DateTime? EndDate
    );
}
