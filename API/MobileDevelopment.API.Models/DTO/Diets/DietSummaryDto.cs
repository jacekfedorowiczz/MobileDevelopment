namespace MobileDevelopment.API.Models.DTO.Diets
{
    public sealed record DietSummaryDto(
        int CaloriesConsumed,
        int CaloriesGoal,
        int Protein,
        int Carbs,
        int Fat);
}
