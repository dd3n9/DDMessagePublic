using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationService.Application.EventProcessing;
using NotificationService.Application.Services;
using NotificationService.Domain.Repositories;
using NotificationService.Infrastructure.Common.AsyncDataServices;
using NotificationService.Infrastructure.Common.Services;
using NotificationService.Infrastructure.Common.Settings;
using NotificationService.Infrastructure.EF.Context;
using NotificationService.Infrastructure.EF.Repositories;
using NotificationService.Infrastructure.EventProcessing;

namespace NotificationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, Common.Services.NotificationService>();
            services.AddSingleton<IEventProcessor, EventProcessor>();

            services.AddScoped<IMailService, GmailService>();

            services.AddHostedService<MessageBusSubscriber>();

            services.AddMsSql(configuration, environment);

            services.AddOptionsSetting(configuration);

            services.Configure<GmailSenderSettings>(configuration.GetSection(GmailSenderSettings.GmailSettingsKey));

            return services;
        }

        private static IServiceCollection AddMsSql(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");

                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("NotificationConnectionString"));
                });
            }
            else
            {
                Console.WriteLine("--> Using Test SqlServer Db");
                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("NotificationConnectionString"));
                });
            }


            return services;
        }

        private static IServiceCollection AddOptionsSetting(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqSettings = new RabbitMQSettings
            {
                HostName = configuration.GetValue<string>("RabbitMQHost"),
                Port = configuration.GetValue<int>("RabbitMQPort")
            };

            services.AddSingleton(Options.Create(rabbitMqSettings));

            return services;
        }
    }
}
