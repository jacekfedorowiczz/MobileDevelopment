using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Services.Services.Calculators;

namespace MobileDevelopment.API.Services.Calculators
{
    public sealed class BmiCalculator : IBmiCalculator
    {
        public decimal Calculate(decimal weightKg, decimal heightCm)
        {
            var heightM = heightCm / 100m;
            return Math.Round(weightKg / (heightM * heightM), 1, MidpointRounding.AwayFromZero);
        }

        public BmiCategory GetCategory(decimal bmi)
        {
            return bmi switch
            {
                < 18.5m => BmiCategory.Underweight,
                < 25m => BmiCategory.Normal,
                < 30m => BmiCategory.Overweight,
                _ => BmiCategory.Obesity
            };
        }
    }
}
