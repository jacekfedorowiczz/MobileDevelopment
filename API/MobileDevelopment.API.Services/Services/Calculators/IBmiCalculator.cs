using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Services.Services.Calculators
{
    public interface IBmiCalculator
    {
        decimal Calculate(decimal weightKg, decimal heightCm);
        BmiCategory GetCategory(decimal bmi);
    }
}
