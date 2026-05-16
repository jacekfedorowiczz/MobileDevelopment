using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Services.Services.Calculators;

namespace MobileDevelopment.API.Services.Calculators
{
    internal sealed class VolumeCalculator : IVolumeCalculator
    {
        public decimal CalculateVolume(IEnumerable<WorkoutSet> sets)
        {
            return sets.Sum(set => set.Weight * set.Reps);
        }
    }
}
