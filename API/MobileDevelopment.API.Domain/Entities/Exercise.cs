using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Exercise : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsCompound { get; set; }

        public ICollection<MuscleGroup> TargetedMuscles { get; set; } = [];
        public ICollection<WorkoutSet> Sets { get; set; } = [];
    }
}
