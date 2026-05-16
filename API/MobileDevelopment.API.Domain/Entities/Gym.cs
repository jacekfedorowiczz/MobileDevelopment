using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Gym : BaseEntity
    {
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Rating { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CreatedByUserId { get; set; }
    }
}
