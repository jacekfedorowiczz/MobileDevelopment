using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Tag : BaseEntity
    {
        public required string Name { get; set; }

        public ICollection<Post> Posts { get; set; } = [];
        public ICollection<Profile> InterestedProfiles { get; set; } = [];
    }
}
