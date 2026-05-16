using FluentValidation;

namespace MobileDevelopment.API.Services.Queries.Calculators
{
    public sealed class CalculateBmiQueryValidator : AbstractValidator<CalculateBmiQuery>
    {
        public CalculateBmiQueryValidator()
        {
            RuleFor(x => x.Dto).NotNull();
            RuleFor(x => x.Dto.WeightKg).InclusiveBetween(20m, 350m);
            RuleFor(x => x.Dto.HeightCm).InclusiveBetween(100m, 250m);
        }
    }

    public sealed class CalculateOneRepMaxQueryValidator : AbstractValidator<CalculateOneRepMaxQuery>
    {
        public CalculateOneRepMaxQueryValidator()
        {
            RuleFor(x => x.Dto).NotNull();
            RuleFor(x => x.Dto.WeightKg).InclusiveBetween(20m, 350m);
            RuleFor(x => x.Dto.Reps).InclusiveBetween(1, 20);
        }
    }

    public sealed class CalculateBmrQueryValidator : AbstractValidator<CalculateBmrQuery>
    {
        public CalculateBmrQueryValidator()
        {
            RuleFor(x => x.Dto).NotNull();
            RuleFor(x => x.Dto.WeightKg).InclusiveBetween(20m, 350m);
            RuleFor(x => x.Dto.HeightCm).InclusiveBetween(100m, 250m);
            RuleFor(x => x.Dto.Age).InclusiveBetween(10, 100);
            RuleFor(x => x.Dto.Gender).IsInEnum();
            RuleFor(x => x.Dto.ActivityFactor).InclusiveBetween(1.2m, 2.5m);
        }
    }

    public sealed class CalculateYmcaBodyFatQueryValidator : AbstractValidator<CalculateYmcaBodyFatQuery>
    {
        public CalculateYmcaBodyFatQueryValidator()
        {
            RuleFor(x => x.Dto).NotNull();
            RuleFor(x => x.Dto.WeightKg).InclusiveBetween(20m, 350m);
            RuleFor(x => x.Dto.WaistCm).InclusiveBetween(40m, 200m);
            RuleFor(x => x.Dto.Gender).IsInEnum();
        }
    }

    public sealed class CalculateIdealWeightQueryValidator : AbstractValidator<CalculateIdealWeightQuery>
    {
        public CalculateIdealWeightQueryValidator()
        {
            RuleFor(x => x.Dto).NotNull();
            RuleFor(x => x.Dto.HeightCm).InclusiveBetween(100m, 250m);
            RuleFor(x => x.Dto.Gender).IsInEnum();
        }
    }
}
