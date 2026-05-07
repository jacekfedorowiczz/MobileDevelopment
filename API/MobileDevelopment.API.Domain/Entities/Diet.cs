using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Diet : BaseEntity
    {
        public int UserId { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive => !EndDate.HasValue || EndDate.Value >= DateTime.UtcNow;

        public User User { get; set; } = null!;
        public ICollection<DietDay> DietDays { get; set; } = [];
    }
}
