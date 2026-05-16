using MobileDevelopment.API.Domain.Base;
using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Exercise : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public ExerciseDifficulty? Difficulty { get; set; }
        public bool IsCompound { get; set; }
        public int? CreatedByUserId { get; set; }
        public bool IsSystem { get; set; } = false;

        public ICollection<MuscleGroup> TargetedMuscles { get; set; } = [];
        public ICollection<WorkoutSet> Sets { get; set; } = [];
    }
}
