using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Services.Services.Calculators;

namespace MobileDevelopment.API.Services.Calculators
{
    public sealed class BmrCalculator : IBmrCalculator
    {
        public decimal Calculate(decimal weightKg, decimal heightCm, int age, Gender gender)
        {
            var genderOffset = gender == Gender.Male ? 5m : -161m;
            var bmr = 10m * weightKg + 6.25m * heightCm - 5m * age + genderOffset;

            return Math.Round(bmr, 0, MidpointRounding.AwayFromZero);
        }
    }
}
