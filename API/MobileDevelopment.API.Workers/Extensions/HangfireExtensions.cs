using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MobileDevelopment.API.Workers.Extensions
{
    public static class HangfireExtensions
    {
        public static IServiceCollection RegisterHangfire(this IServiceCollection services, IConfiguration cfg)
        {
            var connectionString = cfg.GetConnectionString("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Cannot find connection string for Hangfire.");
            }

            services.AddHangfire(cfg =>
                cfg.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString));

            services.AddHangfireServer();
            services.AddScoped<PropertyScraperJob>();

            return services;
        }

        public static IApplicationBuilder UserHangfire(this WebApplication app)
        {
            app.UseHangfireDashboard("/dashboard");

            RecurringJob.AddOrUpdate<PropertyScraperJob>(
                "property-scraper",
                job => job.ExecuteAsync(),
                Cron.Daily()
            );

            return app;
        }
    }
}
