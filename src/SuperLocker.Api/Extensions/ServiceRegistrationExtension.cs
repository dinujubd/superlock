using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using SuperLocker.Api.Models;
using SuperLocker.Api.Validators;
using SuperLocker.CommandHandler;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
using SuperLocker.Core.Validators.Command;
using SuperLocker.Core.Validators.Queries;
using SuperLocker.Crosscuts;
using SuperLocker.DataContext.Adapters;
using SuperLocker.DataContext.Providers;
using SuperLocker.DataContext.Proxies;
using SuperLocker.DataContext.Repositories;
using SuperLocker.QueryHandler;
using System;
using System.Linq;

namespace SuperLocker.Api.Extensions
{
    public static class ServiceRegistrationExtension
    {
        public static void RegisterQueryHandlers(this IServiceCollection services)
        {
            services.AddScoped<IQueryHandler<UnlockActivityQuery, UnlockQueryRespose>, UnlockActivityQueryHandler>();
        }

        public static void RegisterServieBus(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitmqConfig = configuration.GetSection(RabbitMQConfiguration.RabbitMq).Get<RabbitMQConfiguration>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UnlockCommandHandler>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitmqConfig.ConnectionString);
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService(true);
        }

        public static void RegisterAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                var service = configuration.GetSection(ServiceConfigurations.Services).Get<ServiceConfigurations>();
                options.Authority = service.Auth;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "super_lock_api");
                });
            });

            services.AddSingleton(context =>
            {
                return MapAppUserFromContext(context);
            });
        }

        public static void RegisterDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDatabaseConnectionProvider<MySqlConnection>, MySqlConnectionProvider>();
            services.AddScoped(serviceProvider =>
            {
                var provider = serviceProvider.GetRequiredService<IDatabaseConnectionProvider<MySqlConnection>>();

                return new ConnectionPool<MySqlConnection>(() => provider.Get());
            });
            services.AddScoped<ICacheProxy, RedisMySqlCacheProxy>();
            services.AddScoped<ICacheAdapter, RedisCacheAdapter>();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>((options) =>
            {
                return configuration.GetSection("Redis").Get<RedisConfiguration>();
            });

        }

        public static void RegisterRepositoris(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILockRepository, LockRepository>();
        }

        public static void RegisterConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfigurations>(configuration.GetSection(DatabaseConfigurations.Database));
            services.Configure<RabbitMQConfiguration>(configuration.GetSection(RabbitMQConfiguration.RabbitMq));
            services.Configure<ServiceConfigurations>(configuration.GetSection(ServiceConfigurations.Services));
        }

        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UnlockCommand>, UnlockCommandValidator>();
            services.AddScoped<IValidator<UnlockActivityQuery>, UnlockActivityQueryValidator>();
            services.AddScoped<IValidator<UnlockRequest>, UnlockRequestValidator>();
        }

        private static AppUser MapAppUserFromContext(IServiceProvider context)
        {
            var identityClaims = context.GetService<IHttpContextAccessor>().HttpContext?.User?.Claims;
            var userId = identityClaims.FirstOrDefault(x => x.Type == "userId")?.Value;
            var userName = identityClaims.FirstOrDefault(x => x.Type == "userName")?.Value;
            var firstName = identityClaims.FirstOrDefault(x => x.Type == "firstName")?.Value;
            var lastName = identityClaims.FirstOrDefault(x => x.Type == "lastName")?.Value;

            var userInfo = new AppUser()
            {
                UserId = Guid.Parse(userId),
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
            };

            return userInfo;
        }
    }
}
