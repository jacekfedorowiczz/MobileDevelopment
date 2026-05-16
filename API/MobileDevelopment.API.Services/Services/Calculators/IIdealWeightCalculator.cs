using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Services.Services.Calculators
{
    public interface IIdealWeightCalculator
    {
        decimal Calculate(decimal heightCm, Gender gender);
    }
}
