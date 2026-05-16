using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Services.Services.Calculators
{
    public interface IYmcaBodyFatCalculator
    {
        decimal Calculate(decimal weightKg, decimal waistCm, Gender gender);
        BodyFatCategory GetCategory(decimal bodyFatPercentage, Gender gender);
    }
}
