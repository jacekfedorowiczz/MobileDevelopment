using MobileDevelopment.API.Domain.Base;

namespace MobileDevelopment.API.Domain.Entities
{
    public sealed class RefreshToken : BaseEntity
    {
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked => RevokedAt is not null;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
