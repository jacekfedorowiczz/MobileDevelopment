using MobileDevelopment.API.Services.Services.Calculators;

namespace MobileDevelopment.API.Services.Calculators
{
    public sealed class OneRepMaxCalculator : IOneRepMaxCalculator
    {
        public decimal Calculate(decimal weight, int reps)
        {
            if (reps == 1)
            {
                return Math.Round(weight, 1, MidpointRounding.AwayFromZero);
            }

            var oneRepMax = weight * (1m + reps / 30m);
            return Math.Round(oneRepMax, 1, MidpointRounding.AwayFromZero);
        }
    }
}
