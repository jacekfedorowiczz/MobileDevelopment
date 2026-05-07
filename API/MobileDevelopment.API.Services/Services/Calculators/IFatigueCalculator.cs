namespace MobileDevelopment.API.Services.Services.Calculators
{
    public interface IFatigueCalculator
    {
        string GetFatigue(int? globalSessionRpe, double averageSetRpe);
    }
}
