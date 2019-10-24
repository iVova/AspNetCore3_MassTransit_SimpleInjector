using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAspCoreApp.Services;

namespace MyAspCoreApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterModule(new AutofacModule());

            builder.RegisterType<SystemClock>().As<IClock>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //public static IServiceCollection AddCustomMassTransit(IServiceCollection services, Container container)
        //{
        //    // Sources if this method you can find by following url:
        //    // https://github.com/MassTransit/MassTransit/blob/febe5c74b1a7f961efb8f25fd09db43b631632e4/src/Containers/MassTransit.AspNetCoreIntegration/ServiceCollectionExtensions.cs#L52


        //    services.AddMassTransit(sp =>
        //    {
        //        var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
        //        {
        //            var host = cfg.Host(new Uri("rabbitmq://myaspcoreapp.rabbitMq"), hostConfigurator =>
        //            {
        //                hostConfigurator.Username("guest");
        //                hostConfigurator.Password("guest");
        //            });

        //            cfg.ConfigureEndpoints(sp);
        //        });

        //        return bus;
        //    }, scc =>
        //    {
        //        scc.AddConsumersFromNamespaceContaining<SendNotificationOnUserCreatedConsumer>();
        //    });

        //    return services;
        //}
    }
}
