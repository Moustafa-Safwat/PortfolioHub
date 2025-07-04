using System.Reflection;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PortfolioHub.Achievements;
using PortfolioHub.Notification;
using PortfolioHub.Projects;
using PortfolioHub.Users;
using PortfolioHub.Web.Infra.Crosscutting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
    //config.Enrich.
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReverseProxyOnly", policy =>
    {
        policy.WithOrigins(
                "http://portfolio_nginx:8050",
                "http://portfolio_nginx:8020",
                "https://localhost:8050",
                "https://192.168.8.21:8050"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowReverseProxyOnly");

app.UseSerilogRequestLogging();

var hostName = System.Net.Dns.GetHostName();
Log.Information($"Starting up application at Host:{hostName}");

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

app.Run();

public partial class Program { } // For testing purposes only

