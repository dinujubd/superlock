using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MassTransit;
using SuperLocker.CommandHandler;
using FluentValidation.AspNetCore;
using FluentValidation;
using SuperLocker.Core.Command;
using SuperLocker.Core.Repositories;
using SuperLocker.DataContext.Repositories;
using SuperLocker.Core;
using SuperLocker.Core.Query;
using SuperLocker.QueryHandler;
using SuperLocker.DataContext.Providers;
using MySql.Data.MySqlClient;

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
