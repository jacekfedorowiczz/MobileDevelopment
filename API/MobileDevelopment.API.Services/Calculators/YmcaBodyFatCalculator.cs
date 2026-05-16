using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Services.Services.Calculators;

namespace MobileDevelopment.API.Services.Calculators
{
    internal sealed class YmcaBodyFatCalculator : IYmcaBodyFatCalculator
    {
        private const decimal PoundsPerKilogram = 2.20462262185m;
        private const decimal CentimetersPerInch = 2.54m;

        public decimal Calculate(decimal weightKg, decimal waistCm, Gender gender)
        {
            var weightLb = weightKg * PoundsPerKilogram;
            var waistIn = waistCm / CentimetersPerInch;
            var genderConstant = gender == Gender.Male ? -98.42m : -76.76m;
            var bodyFat = (genderConstant + 4.15m * waistIn - 0.082m * weightLb) / weightLb * 100m;

            return Math.Round(Math.Max(0m, bodyFat), 1, MidpointRounding.AwayFromZero);
        }

        public BodyFatCategory GetCategory(decimal bodyFatPercentage, Gender gender)
        {
            return gender == Gender.Male
                ? bodyFatPercentage switch
                {
                    < 6m => BodyFatCategory.VeryLow,
                    < 14m => BodyFatCategory.Athletic,
                    < 18m => BodyFatCategory.Fitness,
                    < 25m => BodyFatCategory.Average,
                    _ => BodyFatCategory.High
                }
                : bodyFatPercentage switch
                {
                    < 14m => BodyFatCategory.VeryLow,
                    < 21m => BodyFatCategory.Athletic,
                    < 25m => BodyFatCategory.Fitness,
                    < 32m => BodyFatCategory.Average,
                    _ => BodyFatCategory.High
                };
        }
    }
}
