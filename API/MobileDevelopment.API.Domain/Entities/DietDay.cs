using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class DietDay : BaseEntity
    {
        public int DietId { get; set; }

        public DateTime Date { get; set; }
        public string? Notes { get; set; }

        public Diet Diet { get; set; } = null!;
        public ICollection<Meal> Meals { get; set; } = [];
    }
}
