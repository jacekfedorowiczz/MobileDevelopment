using MobileDevelopment.API.Domain.Base;
using MobileDevelopment.API.Domain.Enums;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Post : BaseEntity
    {
        public int UserId { get; set; }

        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public FitnessGoal? TargetGoal { get; set; }
        public ICollection<Tag> Tags { get; set; } = [];

        public User User { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = [];
        public ICollection<PostLike> Likes { get; set; } = [];
    }
}
