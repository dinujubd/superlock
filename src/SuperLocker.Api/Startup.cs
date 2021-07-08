using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using StackExchange.Redis.Extensions.Core.Abstractions;
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
using SuperLocker.DataContext.Adapters;
using SuperLocker.DataContext.Providers;
using SuperLocker.DataContext.Repositories;
using SuperLocker.QueryHandler;

namespace SuperLocker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();

            services.AddScoped<IValidator<UnlockCommand>, UnlockCommandValidator>();
            services.AddScoped<IValidator<UnlockRequest>, UnlockRequestValidator>();

            services.AddScoped<IDatabaseConnectionProvider<MySqlConnection>, MySqlConnectionProvider>();

            services.AddScoped(serviceProvider =>
            {
                var provider = serviceProvider.GetRequiredService<IDatabaseConnectionProvider<MySqlConnection>>();

                return new ConnectionPool<MySqlConnection>(() => provider.Get());
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILockRepository, LockRepository>();

            services.AddScoped<IQueryHandler<UnlockActivityQuery, UnlockQueryRespose>, UnlockActivityQueryHandler>();

            services.AddScoped<ICacheAdapter, MySqlCacheAdapter>();

            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>((options) =>
            {
                return Configuration.GetSection("Redis").Get<RedisConfiguration>();
            });

   
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UnlockCommandHandler>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService(true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SuperLocker.Api", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperLocker.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
