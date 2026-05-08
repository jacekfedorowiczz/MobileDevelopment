namespace MobileDevelopment.API.Models.DTO.DietDays
{
    public sealed record DietDayDto(
        int Id,
        int DietId,
        DateTime Date,
        string? Notes
    );

    public sealed record CreateEditDietDayDto(
        int DietId,
        DateTime Date,
        string? Notes
    );
}
