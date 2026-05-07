using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class WorkoutSession : BaseEntity
    {
        public int UserId { get; set; }

        public required string Name { get; set; } 
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? GlobalSessionRpe { get; set; }

        public User User { get; set; } = null!;
        public ICollection<WorkoutSet> Sets { get; set; } = [];
    }
}
