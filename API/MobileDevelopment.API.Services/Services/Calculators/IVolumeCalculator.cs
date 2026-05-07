using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Services.Services.Calculators
{
    public interface IVolumeCalculator
    {
        decimal CalculateVolume(IEnumerable<WorkoutSet> sets);
    }
}
