using Gateway.Api.AsyncDataServices;
using Gateway.Api.Configurations;
using Gateway.Api.Data;
using Gateway.Api.EventProcessing;
using Gateway.Api.Interfaces.Authentication;
using Gateway.Api.Interfaces.Services;
using Gateway.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Gateway.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddSingleton<IEventProcessor, EventProcessor>();

            services.AddHostedService<MessageBusSubscriber>();

            services.Configure<CookiesConfig>(configuration.GetSection(CookiesConfig.SectionName));

            services.AddAuth(configuration);
            services.AddMsSql(configuration, environment);
            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = new JwtConfig();

            configuration.Bind(JwtConfig.SectionName, jwtConfig);
            var key = Encoding.UTF8.GetBytes(jwtConfig.Secret);


            services.AddSingleton(Options.Create(jwtConfig));

            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameter;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[CookiesConfig.SectionName];

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        private static IServiceCollection AddMsSql(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");
                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("YarpConnectionString"));
                });
            }
            else
            {
                Console.WriteLine("--> Using Test SqlServer Db");
                services.AddDbContext<AppDbContext>(ctx =>
                {
                    ctx.UseSqlServer(configuration.GetConnectionString("YarpConnectionString"));
                });
            }

            return services;
        }
    }
}
