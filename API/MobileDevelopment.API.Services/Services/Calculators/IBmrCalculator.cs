using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Services.Services.Calculators
{
    public interface IBmrCalculator
    {
        decimal Calculate(decimal weightKg, decimal heightCm, int age, Gender gender);
    }
}
