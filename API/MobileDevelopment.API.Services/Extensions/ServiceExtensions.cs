using Microsoft.Extensions.DependencyInjection;
using MobileDevelopment.API.Domain.Interfaces.Auth;
using MobileDevelopment.API.Services.Analytics;
using MobileDevelopment.API.Services.Calculators;
using MobileDevelopment.API.Services.Interfaces;
using MobileDevelopment.API.Services.Services;
using MobileDevelopment.API.Services.Services.Background;
using MobileDevelopment.API.Services.Services.Cache;
using MobileDevelopment.API.Services.Services.Calculators;
using MobileDevelopment.API.Services.Services.Facades;
using MobileDevelopment.API.Services.Services.UserContext;

namespace MobileDevelopment.API.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Infrastructure
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICacheService, CacheService>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IMuscleGroupService, MuscleGroupService>();
            services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();
            services.AddScoped<IWorkoutSetService, WorkoutSetService>();
            services.AddScoped<IDietService, DietService>();
            services.AddScoped<IDietDayService, DietDayService>();
            services.AddScoped<IMealService, MealService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostLikeService, PostLikeService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IGymService, GymService>();
            services.AddScoped<IAchievementService, AchievementService>();

            // Calculators
            services.AddScoped<IWorkoutAnalyticsFacade, WorkoutAnalyticsFacade>();
            services.AddScoped<IHealthCalculatorFacade, HealthCalculatorFacade>();
            services.AddScoped<IBmiCalculator, BmiCalculator>();
            services.AddScoped<IBmrCalculator, BmrCalculator>();
            services.AddScoped<IYmcaBodyFatCalculator, YmcaBodyFatCalculator>();
            services.AddScoped<IIdealWeightCalculator, IdealWeightCalculator>();
            services.AddScoped<IVolumeCalculator, VolumeCalculator>();
            services.AddScoped<IOneRepMaxCalculator, OneRepMaxCalculator>();

            // Background services
            services.AddHostedService<TokenCleanupService>();
            services.AddHostedService<AchievementWorker>();

            return services;
        }
    }
}
