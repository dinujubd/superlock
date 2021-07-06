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

            


//  var connection = @"Server=db;Database=master;User=sa;Password=Your_password123;";

    // This line uses 'UseSqlServer' in the 'options' parameter
    // with the connection string defined above.



            services.AddTransient<IValidator<LockCommand>, LockCommandValidator>();


            services.AddScoped<ILockRepository, LockRepository>();
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<LockCommandHandler>();

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
