using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    public sealed class GymConfiguration : IEntityTypeConfiguration<Gym>
    {
        public void Configure(EntityTypeBuilder<Gym> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(g => g.Street)
                   .IsRequired();

            builder.Property(g => g.City)
                    .IsRequired();

            builder.Property(g => g.ZipCode)
                    .IsRequired();


            builder.Property(g => g.Description)
                   .HasMaxLength(1000);

            builder.Property(g => g.IsActive)
                   .HasDefaultValue(true);
        }
    }
}
