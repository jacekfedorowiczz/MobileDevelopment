using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobileDevelopment.API.Domain.Entities;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Interfaces;
using MobileDevelopment.API.Persistence.Repositories;
using MobileDevelopment.API.Persistence.Seeding;

namespace MobileDevelopment.API.Persistence.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MsSqlDb");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Cannot find a valid connection string.");
            }

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddDbContext<SystemContext>(options =>
            {
                options.UseSqlServer(connectionString, optionsAction =>
                {
                    optionsAction.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                    );
                });

                options.UseSeeding((context, _) =>
                {
                    var hasher = context.GetInfrastructure().GetService<IPasswordHasher<User>>()
                                 ?? new PasswordHasher<User>();
                    DatabaseSeeder.Seed(context, hasher);
                    context.SaveChanges();
                });

                options.UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    var hasher = context.GetInfrastructure().GetService<IPasswordHasher<User>>()
                                 ?? new PasswordHasher<User>();
                    await DatabaseSeeder.SeedAsync(context, hasher, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                });
            });

            services.AddScoped(typeof(IBaseEntityRepository<>), typeof(Repository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IMuscleGroupRepository, MuscleGroupRepository>();
            services.AddScoped<IWorkoutSessionRepository, WorkoutSessionRepository>();
            services.AddScoped<IWorkoutSetRepository, WorkoutSetRepository>();
            services.AddScoped<IDietRepository, DietRepository>();
            services.AddScoped<IDietDayRepository, DietDayRepository>();
            services.AddScoped<IMealRepository, MealRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostLikeRepository, PostLikeRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            return services;
        }
    }
}
