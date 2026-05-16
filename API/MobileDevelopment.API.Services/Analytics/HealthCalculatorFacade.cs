using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Models.DTO.Calculators;
using MobileDevelopment.API.Services.Services.Calculators;
using MobileDevelopment.API.Services.Services.Facades;

namespace MobileDevelopment.API.Services.Analytics
{
    internal sealed class HealthCalculatorFacade : IHealthCalculatorFacade
    {
        private readonly IBmiCalculator _bmiCalculator;
        private readonly IOneRepMaxCalculator _oneRepMaxCalculator;
        private readonly IBmrCalculator _bmrCalculator;
        private readonly IYmcaBodyFatCalculator _ymcaBodyFatCalculator;
        private readonly IIdealWeightCalculator _idealWeightCalculator;

        public HealthCalculatorFacade(
            IBmiCalculator bmiCalculator,
            IOneRepMaxCalculator oneRepMaxCalculator,
            IBmrCalculator bmrCalculator,
            IYmcaBodyFatCalculator ymcaBodyFatCalculator,
            IIdealWeightCalculator idealWeightCalculator)
        {
            _bmiCalculator = bmiCalculator;
            _oneRepMaxCalculator = oneRepMaxCalculator;
            _bmrCalculator = bmrCalculator;
            _ymcaBodyFatCalculator = ymcaBodyFatCalculator;
            _idealWeightCalculator = idealWeightCalculator;
        }

        public BmiResultDto CalculateBmi(BmiRequestDto dto)
        {
            ValidateWeight(dto.WeightKg);
            ValidateHeight(dto.HeightCm);

            var bmi = _bmiCalculator.Calculate(dto.WeightKg, dto.HeightCm);
            var heightM = dto.HeightCm / 100m;
            return new BmiResultDto(
                bmi,
                _bmiCalculator.GetCategory(bmi),
                Math.Round(18.5m * heightM * heightM, 1, MidpointRounding.AwayFromZero),
                Math.Round(24.9m * heightM * heightM, 1, MidpointRounding.AwayFromZero));
        }

        public OneRepMaxResultDto CalculateOneRepMax(OneRepMaxRequestDto dto)
        {
            ValidateWeight(dto.WeightKg);
            if (dto.Reps is < 1 or > 20)
            {
                throw new ArgumentException("Reps must be in the range of 1-20.");
            }

            return new OneRepMaxResultDto(
                _oneRepMaxCalculator.Calculate(dto.WeightKg, dto.Reps),
                CalculatorFormula.Epley);
        }

        public BmrResultDto CalculateBmr(BmrRequestDto dto)
        {
            ValidateWeight(dto.WeightKg);
            ValidateHeight(dto.HeightCm);

            if (dto.Age is < 10 or > 100)
            {
                throw new ArgumentException("Age must be in the range of 10-100 years.");
            }

            if (dto.ActivityFactor is < 1.2m or > 2.5m)
            {
                throw new ArgumentException("Activity factor must be in the range of 1.2-2.5.");
            }

            var bmr = _bmrCalculator.Calculate(dto.WeightKg, dto.HeightCm, dto.Age, dto.Gender);
            return new BmrResultDto(
                bmr,
                Math.Round(bmr * dto.ActivityFactor, 0, MidpointRounding.AwayFromZero),
                CalculatorFormula.MifflinStJeor);
        }

        public YmcaBodyFatResultDto CalculateYmcaBodyFat(YmcaBodyFatRequestDto dto)
        {
            ValidateWeight(dto.WeightKg);
            if (dto.WaistCm is < 40m or > 200m)
            {
                throw new ArgumentException("Waist circumference must be in the range of 40-200 cm.");
            }

            var bodyFat = _ymcaBodyFatCalculator.Calculate(dto.WeightKg, dto.WaistCm, dto.Gender);
            return new YmcaBodyFatResultDto(
                bodyFat,
                _ymcaBodyFatCalculator.GetCategory(bodyFat, dto.Gender),
                CalculatorFormula.Ymca);
        }

        public IdealWeightResultDto CalculateIdealWeight(IdealWeightRequestDto dto)
        {
            ValidateHeight(dto.HeightCm);

            var idealWeight = _idealWeightCalculator.Calculate(dto.HeightCm, dto.Gender);
            return new IdealWeightResultDto(
                idealWeight,
                Math.Round(idealWeight * 0.9m, 1, MidpointRounding.AwayFromZero),
                Math.Round(idealWeight * 1.1m, 1, MidpointRounding.AwayFromZero),
                CalculatorFormula.Devine);
        }

        private static void ValidateWeight(decimal weightKg)
        {
            if (weightKg is < 20m or > 350m)
            {
                throw new ArgumentException("Weight must be in the range of 20-350 kg.");
            }
        }

        private static void ValidateHeight(decimal heightCm)
        {
            if (heightCm is < 100m or > 250m)
            {
                throw new ArgumentException("Height must be in the range of 100-250 cm.");
            }
        }
    }
}
