using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.EventProcessing;
using UserService.Repositories;

namespace UserService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddSingleton<IEventProcessor, EventProcessor>();

            services.AddHostedService<MessageBusSubscriber>();

            services.AddMappings();

            services.AddMsSql(configuration, environment);

            return services;
        }

        private static IServiceCollection AddMsSql(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");

                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("UserConnectionString"));
                });
            }
            else
            {
                Console.WriteLine("--> Using Test SqlServer Db");
                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("UserConnectionString"));
                });
            }


            return services;
        }

        private static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
