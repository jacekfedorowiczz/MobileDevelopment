namespace MobileDevelopment.API.Models.DTO.Meals
{
    public sealed record MealDto(
        int Id,
        int DietDayId,
        string Name,
        TimeSpan? Time,
        decimal TotalCalories,
        decimal Protein,
        decimal Carbs,
        decimal Fats
    );

    public sealed record CreateEditMealDto(
        int DietDayId,
        string Name,
        TimeSpan? Time,
        decimal TotalCalories,
        decimal Protein,
        decimal Carbs,
        decimal Fats
    );
}
