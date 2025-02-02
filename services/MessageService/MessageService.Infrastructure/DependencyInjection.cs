using MessageService.Application.Common.AsyncDataServices;
using MessageService.Application.Common.Services;
using MessageService.Application.Common.SyncDataServices.Grpc;
using MessageService.Domain.Factories.Messages;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Common.AsyncDataServices;
using MessageService.Infrastructure.Common.Settings;
using MessageService.Infrastructure.Common.SyncDataServices.Grpc;
using MessageService.Infrastructure.EF.Context;
using MessageService.Infrastructure.EF.Interceptors;
using MessageService.Infrastructure.EF.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace MessageService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
            ConfigurationManager configuration, 
            IWebHostEnvironment environment)
        {
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, Common.Services.MessageService>();
            services.AddScoped<IUserDataClient, UserDataClient>();
            services.AddScoped<IScheduledTaskDataClient, ScheduledTaskDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();

            services.AddScoped<IMessageFactory, MessageFactory>();

            services.AddScoped<PublishDomainEventsInterceptor>();

            services.AddMsSql(configuration, environment);

            services.AddOptionsSetting(configuration);

            return services;
        }

        private static IServiceCollection AddMsSql(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");

                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("MessageConnectionString"));
                });
            }
            else
            {
                Console.WriteLine("--> Using Test SqlServer Db");
                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("MessageConnectionString"));
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

            var grpcUser = new GrpcUserSettings
            {
                GrpcUser = configuration.GetValue<string>(GrpcUserSettings.SectionName)
            };

            services.AddSingleton(Options.Create(grpcUser));

            var grpcSchedulerTask = new GrpcScheduledTaskSettings
            {
                GrpcScheduledTask = configuration.GetValue<string>(GrpcScheduledTaskSettings.SectionName)
            };

            services.AddSingleton(Options.Create(grpcSchedulerTask));

            return services;
        }
    }
}
