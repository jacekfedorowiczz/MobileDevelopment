namespace MobileDevelopment.API.Models.DTO.DietDays
{
    public sealed record DietDayDto(
        int Id,
        int DietId,
        DateTime Date,
        string? Notes,
        MobileDevelopment.API.Models.DTO.Diets.DietDto? Diet = null,
        System.Collections.Generic.ICollection<MobileDevelopment.API.Models.DTO.Meals.MealDto>? Meals = null
    );

    public sealed record CreateEditDietDayDto(
        int DietId,
        DateTime Date,
        string? Notes
    );

    public sealed record CreateDietDayWithMealsDto(
        int DietId,
        DateTime Date,
        string? Notes,
        System.Collections.Generic.IEnumerable<MobileDevelopment.API.Models.DTO.Meals.CreateEditMealDto>? Meals
    );
}
