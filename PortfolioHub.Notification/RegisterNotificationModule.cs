using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioHub.Notification.Application;
using PortfolioHub.Notification.Domain.Interfaces;
using PortfolioHub.Notification.Infrastructure;
using PortfolioHub.Notification.Infrastructure.Backgroundworker;
using PortfolioHub.Notification.Infrastructure.Context;
using PortfolioHub.Notification.Infrastructure.EFRepository;
using PortfolioHub.Notification.Infrastructure.Services;

namespace PortfolioHub.Notification;

public static class RegisterNotificationModule
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection service,
      IConfiguration configuration, IList<Assembly> assemblies)
    {
        service.AddSqlServer<OutboxDbContext>(configuration.GetConnectionString("OutboxDb"));

        service.AddScoped<IContactMessageFormatter, ContactMessageFormatter>();
        service.AddScoped<IQueueEmailMessage, EFQueueEmailMessage>();
        service.AddScoped<ISendEmailFromOutboxService, SendEmailFromOutboxService>();
        service.AddScoped<IGetUnsentEmailMessages, EFGetUnsentEmailMessages>();
        service.AddScoped<ISendEmail, MimeKitEmailSender>();

        service.AddHostedService<EmailSendingBackgroundService>();

        assemblies.Add(typeof(RegisterNotificationModule).Assembly);
        return service;
    }
}
