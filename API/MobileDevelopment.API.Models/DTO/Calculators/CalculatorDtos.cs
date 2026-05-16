using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Models.DTO.Calculators
{
    public sealed record BmiRequestDto(decimal WeightKg, decimal HeightCm);

    public sealed record BmiResultDto(
        decimal Bmi,
        BmiCategory Category,
        decimal HealthyWeightMinKg,
        decimal HealthyWeightMaxKg);

    public sealed record OneRepMaxRequestDto(decimal WeightKg, int Reps);

    public sealed record OneRepMaxResultDto(
        decimal OneRepMaxKg,
        CalculatorFormula Formula);

    public sealed record BmrRequestDto(
        decimal WeightKg,
        decimal HeightCm,
        int Age,
        Gender Gender,
        decimal ActivityFactor = 1.2m);

    public sealed record BmrResultDto(
        decimal BasalMetabolicRate,
        decimal MaintenanceCalories,
        CalculatorFormula Formula);

    public sealed record YmcaBodyFatRequestDto(
        decimal WeightKg,
        decimal WaistCm,
        Gender Gender);

    public sealed record YmcaBodyFatResultDto(
        decimal BodyFatPercentage,
        BodyFatCategory Category,
        CalculatorFormula Formula);

    public sealed record IdealWeightRequestDto(
        decimal HeightCm,
        Gender Gender);

    public sealed record IdealWeightResultDto(
        decimal IdealWeightKg,
        decimal RangeMinKg,
        decimal RangeMaxKg,
        CalculatorFormula Formula);
}
