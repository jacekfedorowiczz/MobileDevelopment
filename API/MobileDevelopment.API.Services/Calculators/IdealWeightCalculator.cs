using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Services.Services.Calculators;

namespace MobileDevelopment.API.Services.Calculators
{
    internal sealed class IdealWeightCalculator : IIdealWeightCalculator
    {
        private const decimal CentimetersPerInch = 2.54m;

        public decimal Calculate(decimal heightCm, Gender gender)
        {
            var heightInches = heightCm / CentimetersPerInch;
            var inchesOverFiveFeet = Math.Max(0m, heightInches - 60m);
            var baseWeight = gender == Gender.Male ? 50m : 45.5m;
            const decimal weightPerInch = 2.3m;

            return Math.Round(baseWeight + weightPerInch * inchesOverFiveFeet, 1, MidpointRounding.AwayFromZero);
        }
    }
}
