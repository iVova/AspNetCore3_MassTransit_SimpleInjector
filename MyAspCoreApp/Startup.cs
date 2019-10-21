using MassTransit;
using MassTransit.AspNetCoreIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyAspCoreApp.Consumers;
using MyAspCoreApp.Services;
using SimpleInjector;
using System;

namespace MyAspCoreApp
{
    public class Startup
    {
        private Container _container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSimpleInjector(_container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    .AddControllerActivation();
            });

            AddCustomMassTransit(services, _container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // !!!!
            ConfigureSimpleInjector(app, _container);



            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static IApplicationBuilder ConfigureSimpleInjector(IApplicationBuilder app, Container container)
        {
            // UseSimpleInjector() enables framework services to be injected into
            // application components, resolved by Simple Injector.
            app.UseSimpleInjector(container, options =>
            {
                // Add custom Simple Injector-created middleware to the ASP.NET pipeline.
                // options.UseMiddleware<CustomMiddleware1>(app);

                // Optionally, allow application components to depend on the
                // non-generic Microsoft.Extensions.Logging.ILogger abstraction.
                options.UseLogging();
            });

            InitializeContainer(container);

            // Always verify the container
            container.Verify();

            return app;
        }

        private static void InitializeContainer(Container container)
        {
            // Add application services. For instance:
            container.Register<IClock, SystemClock>();
        }

        public static IServiceCollection AddCustomMassTransit(IServiceCollection services, Container container)
        {
            // Sources if this method you can find by following url:
            // https://github.com/MassTransit/MassTransit/blob/febe5c74b1a7f961efb8f25fd09db43b631632e4/src/Containers/MassTransit.AspNetCoreIntegration/ServiceCollectionExtensions.cs#L52


            services.AddMassTransit(sp =>
            {
                var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri("rabbitmq://myaspcoreapp.rabbitMq"), hostConfigurator =>
                    {
                        hostConfigurator.Username("guest");
                        hostConfigurator.Password("guest");
                    });

                    cfg.ConfigureEndpoints(sp);
                });

                return bus;
            }, scc =>
            {
                scc.AddConsumersFromNamespaceContaining<SendNotificationOnUserCreatedConsumer>();
            });

            return services;
        }
    }
}
