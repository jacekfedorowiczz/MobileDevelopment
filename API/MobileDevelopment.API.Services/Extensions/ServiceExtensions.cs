using Microsoft.Extensions.DependencyInjection;
using MobileDevelopment.API.Services.Analytics;
using MobileDevelopment.API.Services.Calculators;
using MobileDevelopment.API.Services.Services.Background;
using MobileDevelopment.API.Services.Services.Calculators;
using MobileDevelopment.API.Services.Services.Facades;

namespace MobileDevelopment.API.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();


            services.AddScoped<IWorkoutAnalyticsFacade, WorkoutAnalyticsFacade>();
            services.AddScoped<IFatigueCalculator, FatigueCalculator>();
            services.AddScoped<IVolumeCalculator, VolumeCalculator>();
            services.AddScoped<IOneRepMaxCalculator, OneRepMaxCalculator>();

            // IProgressEngine - silnik progresji

            // IRecoveryCalculator - kalkulator regeneracji i objętości

            // IStrengthCalculator - kalkulator siły względnej 

            // ICalorieCalculator - kalkulator kalorii 


            services.AddHostedService<TokenCleanupService>();
            return services;
        }
    }
}
