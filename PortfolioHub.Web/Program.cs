using System.Reflection;
using System.Security.Claims;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PortfolioHub.Achievements;
using PortfolioHub.Notification;
using PortfolioHub.Projects;
using PortfolioHub.Users;
using PortfolioHub.Web.Infra;
using PortfolioHub.Web.Infra.Crosscutting;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Ensure Serilog reads configuration from appsettings.json/appsettings.Production.json only
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

IList<Assembly> assemblies = [typeof(Program).Assembly];
builder.Services.AddUsersModule(builder.Configuration, assemblies)
    .AddProjectsModule(builder.Configuration, assemblies)
    .AddAchievementsModule(builder.Configuration, assemblies)
    .AddNotificationModule(builder.Configuration, assemblies);
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Auth:JwtSecret"] ??
                throw new InvalidOperationException("JWT Secret is not configured."))),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization()
                .AddFastEndpoints();
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(assemblies.ToArray());
});

// Register logging pipeline
builder.Services.AddScoped(
    typeof(IPipelineBehavior<,>),
    typeof(LoggingPipeLineBehaviour<,>));


// Add CORS policy to allow any method, header, and origin
var allowedOrigins = builder.Configuration["ALLOWED_ORIGINS"]?
    .Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReverseProxyOnly", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowReverseProxyOnly");

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("UserId",
            httpContext.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous");
    };
});

var hostName = System.Net.Dns.GetHostName();
var environment = app.Environment.EnvironmentName;
Log.Information($"Starting up application at Host:{hostName} at:{environment}");

app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints(options =>
   {
       options.Endpoints.RoutePrefix = "api";
       options.Endpoints
              .Configurator = (configure) =>
              {
                  configure.AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
              };
   });

app.ApplyPendingMigrations(assemblies);

app.Run();

public partial class Program { } // For testing purposes only

