using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    internal sealed class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(4096);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.TargetGoal)
                .HasConversion<string>()
                .HasMaxLength(32);

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity(j => j.ToTable("PostTags"));
        }
    }
}
