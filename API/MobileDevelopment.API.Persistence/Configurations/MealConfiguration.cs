using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Configurations
{
    internal sealed class MealConfiguration : IEntityTypeConfiguration<Meal>
    {
        public void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(m => m.TotalCalories)
                .HasPrecision(8, 2);

            builder.Property(m => m.Protein)
                .HasPrecision(6, 2);

            builder.Property(m => m.Carbs)
                .HasPrecision(6, 2);

            builder.Property(m => m.Fats)
                .HasPrecision(6, 2);

            builder.HasOne(m => m.DietDay)
                .WithMany(dd => dd.Meals)
                .HasForeignKey(m => m.DietDayId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
