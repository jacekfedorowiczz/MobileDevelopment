using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class Comment : BaseEntity
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
