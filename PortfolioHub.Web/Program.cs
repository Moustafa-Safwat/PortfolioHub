using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PortfolioHub.Projects;
using PortfolioHub.Users;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddSingleton(Log.Logger);

Log.Information("Starting up application");

IList<Assembly> assemblies = [typeof(Program).Assembly];
builder.Services.AddUsersModule(builder.Configuration, assemblies)
    .AddProjectsModule(builder.Configuration, assemblies);
builder.Services
    .AddAuthenticationJwtBearer(options =>
    {
        options.SigningKey = builder.Configuration["Auth:JwtSecret"];
    })
    .AddAuthorization()
    .AddFastEndpoints();
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(assemblies.ToArray());
});
// Have to define the Auth schema of the FastEndpoints in the Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
});
// ----------------------------------------
// ===== Register pipeline behaviours =====
// ----------------------------------------
// 1) Register logging pipeline
//builder.Services.AddLoggingBehaviour();
// 2) Register validation pipeline
//builder.Services.AddValidationBehaviour();
// ----------------------------------------


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) { }

app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints();

app.Run();

public partial class Program { } // For testing purposes only

