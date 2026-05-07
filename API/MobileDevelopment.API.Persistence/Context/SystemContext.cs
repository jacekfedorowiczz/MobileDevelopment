using Microsoft.EntityFrameworkCore;
using MobileDevelopment.API.Domain.Entities;

namespace MobileDevelopment.API.Persistence.Context
{
    public sealed class SystemContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<WorkoutSet> WorkoutSets { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<DietDay> DietDays { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public SystemContext(DbContextOptions<SystemContext> options) : base(options)
        {     
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
