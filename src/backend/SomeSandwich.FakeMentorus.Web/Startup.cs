using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.Infrastructure.DataAccess;
using SomeSandwich.FakeMentorus.Web.Hubs;
using SomeSandwich.FakeMentorus.Web.Infrastructure.DependencyInjection;
using SomeSandwich.FakeMentorus.Web.Infrastructure.Middlewares;
using SomeSandwich.FakeMentorus.Web.Infrastructure.Settings;
using SomeSandwich.FakeMentorus.Web.Infrastructure.Startup;
using SomeSandwich.FakeMentorus.Web.Infrastructure.Startup.HealthCheck;
using SomeSandwich.FakeMentorus.Web.Infrastructure.Startup.Swagger;

namespace SomeSandwich.FakeMentorus.Web;

/// <summary>
/// Entry point for ASP.NET Core app.
/// </summary>
public class Startup
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// Entry point for web application.
    /// </summary>
    /// <param name="configuration">Global configuration.</param>
    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Configure application services on startup.
    /// </summary>
    /// <param name="services">Services to configure.</param>
    /// <param name="environment">Application environment.</param>
    public void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
    {
        // Swagger.
        services.AddSwaggerGen(new SwaggerGenOptionsSetup().Setup);

        // CORS.
        string[]? frontendOrigin = null;
        // TODO: uncomment if you need to specify additional FrontendOrigins for CORS (if you have an Angular/React/etc project).
        frontendOrigin = Saritasa.Tools.Common.Utils.StringUtils.NullSafe(configuration["AppSettings:FrontendOrigin"])
            .Split(';', StringSplitOptions.RemoveEmptyEntries);
        services.AddCors(new CorsOptionsSetup(
            environment.IsDevelopment(),
            frontendOrigin
        ).Setup);

        // Health check.
        var databaseConnectionString = configuration.GetConnectionString("AppDatabase")
                                       ?? throw new ArgumentNullException(
                                           "ConnectionStrings:AppDatabase",
                                           "Database connection string is not initialized");
        services.AddHealthChecks()
            .AddNpgSql(databaseConnectionString);

        // MVC.
        services
            .AddControllers()
            .AddJsonOptions(new JsonOptionsSetup().Setup);
        services.Configure<ApiBehaviorOptions>(new ApiBehaviorOptionsSetup().Setup);

        // We need to set the application name to data protection, since the default token
        // provider uses this data to create the token. If it is not specified explicitly,
        // tokens from different instances will be incompatible.
        services.AddDataProtection().SetApplicationName("Application")
            .PersistKeysToDbContext<AppDbContext>();

        // Identity.
        services.AddIdentity<User, AppIdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(new IdentityOptionsSetup().Setup);

        // JWT.
        var jwtSecretKey = configuration["Jwt:SecretKey"] ??
                           throw new ArgumentNullException("Jwt:SecretKey");
        var jwtIssuer = configuration["Jwt:Issuer"] ??
                        throw new ArgumentNullException("Jwt:Issuer");
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(new JwtBearerOptionsSetup(
                jwtSecretKey,
                jwtIssuer).Setup
            );

        // Database.
        services.AddDbContext<AppDbContext>(
            new DbContextOptionsSetup(databaseConnectionString).Setup);
        services.AddAsyncInitializer<DatabaseInitializer>();

        // Logging.
        services.AddLogging(new LoggingOptionsSetup(configuration, environment).Setup);

        // Application settings.
        services.AddSingleton<IAppSettings, AppSettings>(_ =>
        {
            var appSettings = new AppSettings();
            configuration.GetSection("Application").Bind(appSettings);
            return appSettings;
        });

        services.AddSignalR();

        // HTTP client.
        services.AddHttpClient();

        // Caching.
        services.AddMemoryCache();

        // Other dependencies.
        AutoMapperModule.Register(services);
        ApplicationModule.Register(services, configuration);
        MediatRModule.Register(services);
        SystemModule.Register(services);
        MailModule.Register(services, configuration);
    }

    /// <summary>
    /// Configure web application.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <param name="environment">Application environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        // Swagger
        // if (!environment.IsProduction())
        // {
        app.UseSwagger();
        app.UseSwaggerUI(new SwaggerUIOptionsSetup().Setup);

        // }

        // Custom middlewares.
        app.UseMiddleware<ApiExceptionMiddleware>();

        // MVC.
        app.UseRouting();

        // CORS.
        app.UseCors(CorsOptionsSetup.CorsPolicyName);
        app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            HealthCheckModule.Register(endpoints);
            endpoints.Map("/", context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });
            endpoints.MapControllers();
        });
    }
}
