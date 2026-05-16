namespace MobileDevelopment.API.Models.DTO.Diets
{
    public sealed record DietDto(
        int Id,
        int UserId,
        string Name,
        string? Description,
        DateTime StartDate,
        DateTime? EndDate,
        bool IsActive,
        MobileDevelopment.API.Models.DTO.Users.UserDto? User = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.DietDays.DietDayDto>? DietDays = null
    );

    public sealed record CreateEditDietDto(
        int UserId,
        string Name,
        string? Description,
        DateTime StartDate,
        DateTime? EndDate
    );

    public sealed record CreateDietWithDaysDto(
        int UserId,
        string Name,
        string? Description,
        DateTime StartDate,
        DateTime? EndDate,
        System.Collections.Generic.IEnumerable<MobileDevelopment.API.Models.DTO.DietDays.CreateDietDayWithMealsDto>? DietDays
    );
}
