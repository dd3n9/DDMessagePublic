using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using SchedulerService.Application.Common.AsyncDataServices;
using SchedulerService.Application.Common.Services;
using SchedulerService.Application.EventProcessing;
using SchedulerService.Domain.Repositories;
using SchedulerService.Infrastructure.Common.AsyncDataServices;
using SchedulerService.Infrastructure.Common.Services;
using SchedulerService.Infrastructure.Common.Settings;
using SchedulerService.Infrastructure.EF.Context;
using SchedulerService.Infrastructure.EF.Inteseptors;
using SchedulerService.Infrastructure.EF.Repositories;
using SchedulerService.Infrastructure.EventProcessing;
using SchedulerService.Infrastructure.Jobs;

namespace SchedulerService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
             IConfiguration configuration,
             IWebHostEnvironment environment)
        {
            services.AddScoped<IScheduledTaskRepository, ScheduledTaskRepository>();
            services.AddScoped<IScheduledTaskService, ScheduledTaskService>();
            services.AddSingleton<IEventProcessor, EventProcessor>();

            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddHostedService<MessageBusSubscriber>();

            services.AddScoped<PublishDomainEventsInterceptor>();

            services.AddMsSql(configuration, environment);

            services.AddOptionsSetting(configuration);

            services.AddQuartzJob(configuration);

            return services;
        }

        private static IServiceCollection AddMsSql(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");

                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("SchedulerConnectionString"));
                });
            }
            else
            {
                Console.WriteLine("--> Using Test SqlServer Db");
                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("SchedulerConnectionString"));
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

        private static IServiceCollection AddQuartzJob(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(opt =>
            {
                opt.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = new JobKey("CheckTaskJobSettings");
                opt.AddJob<CheckScheduledTasksJob>(options => options.WithIdentity(jobKey));
                opt.AddTrigger(options =>
                {
                    options.ForJob(jobKey)
                        .WithIdentity("CheckTaskJobSettings-trigger")
                        .WithCronSchedule(configuration.GetSection("CheckTaskJobSettings:CronSchedule")
                            .Value ?? "0 * * * * ?");
                });
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
