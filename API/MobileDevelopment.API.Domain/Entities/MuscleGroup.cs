using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class MuscleGroup : BaseEntity
    {
        public required string Name { get; set; } 
        public ICollection<Exercise> Exercises { get; set; } = [];
    }
}
