namespace MobileDevelopment.API.Services.Services.Calculators
{
    public interface IOneRepMaxCalculator
    {
        decimal Calculate(decimal weight, int reps);
    }
}
