using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class WorkoutSet : BaseEntity
    {
        public int WorkoutSessionId { get; set; } 
        public int ExerciseId { get; set; }

        public int SetNumber { get; set; }
        public decimal Weight { get; set; }
        public int Reps { get; set; }
        public int? Rpe { get; set; }

        public WorkoutSession WorkoutSession { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;
    }
}
