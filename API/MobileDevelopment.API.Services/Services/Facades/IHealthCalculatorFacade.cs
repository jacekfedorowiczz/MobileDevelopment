using MobileDevelopment.API.Models.DTO.Calculators;

namespace MobileDevelopment.API.Services.Services.Facades
{
    public interface IHealthCalculatorFacade
    {
        BmiResultDto CalculateBmi(BmiRequestDto dto);
        OneRepMaxResultDto CalculateOneRepMax(OneRepMaxRequestDto dto);
        BmrResultDto CalculateBmr(BmrRequestDto dto);
        YmcaBodyFatResultDto CalculateYmcaBodyFat(YmcaBodyFatRequestDto dto);
        IdealWeightResultDto CalculateIdealWeight(IdealWeightRequestDto dto);
    }
}
