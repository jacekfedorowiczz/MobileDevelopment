using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Meal : BaseEntity
    {
        public int DietDayId { get; set; }

        public required string Name { get; set; }
        public TimeSpan? Time { get; set; }
        public decimal TotalCalories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fats { get; set; }

        public DietDay DietDay { get; set; } = null!;
    }
}
