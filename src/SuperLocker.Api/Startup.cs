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
using SuperLocker.CommandHandler;
using SuperLocker.Core;
using SuperLocker.Core.Command;
using SuperLocker.Core.Query;
using SuperLocker.Core.Repositories;
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

            services.AddTransient<IValidator<UnlockCommand>, UnlockCommandValidator>();

            services.AddSingleton<IDatabaseConnectionProvider<MySqlConnection>, MySqlConnectionProvider>();

            services.AddSingleton<ConnectionPool<MySqlConnection>>(serviceProvider =>
            {
                var provider = serviceProvider.GetRequiredService<IDatabaseConnectionProvider<MySqlConnection>>();

                return new ConnectionPool<MySqlConnection>(() => provider.Get());
            });

            services.AddScoped<ILockRepository, LockRepository>();

            services.AddScoped<IQueryHandler<UnlockActivityQuery, UnlockQueryRespose>, UnlockActivityQueryHandler>();

            services.AddSingleton<ICacheAdapter, MySqlCacheAdapter>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("Redis:Endpoint");
                options.InstanceName = Configuration.GetValue<string>("Redis:Instance");
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
