using Asp.Versioning; // Wymagane dla nowej wersji ApiVersioning
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileDevelopment.API.Domain.Auth;
using MobileDevelopment.API.Domain.Constants;
using MobileDevelopment.API.Domain.Enums;
using MobileDevelopment.API.Middlewares;
using MobileDevelopment.API.Persistence.Context;
using MobileDevelopment.API.Persistence.Extensions;
using MobileDevelopment.API.Services.Communication;
using MobileDevelopment.API.Services.Extensions;
using MobileDevelopment.API.Services.Queries.GetUserQuery;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

namespace MobileDevelopment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            services.AddSerilog();

            var jwtSettings = new JwtSettings { SecretKey = "", Issuer = "", Audience = "" };
            builder.Configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
            services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

            services.AddControllers();
            services.AddOpenApi();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Constants.Bearer;
                options.DefaultScheme = Constants.Bearer;
                options.DefaultChallengeScheme = Constants.Bearer;
            }).AddJwtBearer(cfg =>
            {
                #if !DEBUG
                cfg.RequireHttpsMetadata = true;
                #endif
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };

                cfg.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("AccessToken"))
                        {
                            context.Token = context.Request.Cookies["AccessToken"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorizationBuilder()
                .AddPolicy("RequireAdminRole", policy => policy.RequireRole(nameof(Role.Administrator)));
            services.AddAntiforgery();

            #if DEBUG
            services.AddCors(options =>
            {
                options.AddPolicy(Constants.DebugClient, policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowAnyOrigin();
                });
            });
            #endif

            // Rate limiting
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10;
                    limiterOptions.Window = TimeSpan.FromSeconds(10);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 5;
                });
            });

            services.AddResponseCompression();
            services.AddOutputCache();

            // API Versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<ErrorHandlingMiddleware>();
            services.AddTransient<RequestTimeMiddleware>();

            services
                .RegisterPersistence(builder.Configuration)
                .RegisterServices();

            var servicesAssembly = typeof(GetUserQuery).Assembly;
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(servicesAssembly));
            services.AddValidatorsFromAssembly(servicesAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            var app = builder.Build();
            var isDevelopmentMode = app.Environment.IsDevelopment();

            if (isDevelopmentMode)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            app.UseHttpsRedirection();
            app.UseResponseCompression();

            app.UseRouting();

            #if DEBUG
            app.UseCors(Constants.DebugClient);
            #endif

            app.UseOutputCache();
            app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery(); 

            app.MapStaticAssets();
            app.MapControllers();

            if (isDevelopmentMode)
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.WithTitle("API Docs");
                    options.WithTheme(ScalarTheme.DeepSpace);
                    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            using (var scope = app.Services.CreateScope())
            {
                var scopeServices = scope.ServiceProvider;
                try
                {
                    var context = scopeServices.GetRequiredService<SystemContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An unexpected error occurred during the automatic database migration.");
                }
            }

            app.Run();
        }
    }
}