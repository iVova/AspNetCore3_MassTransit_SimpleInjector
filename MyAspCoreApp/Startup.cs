using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyAspCoreApp.Consumers;
using MyAspCoreApp.Services;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;

namespace MyAspCoreApp
{
    public class Startup
    {
        private readonly Container _container = new Container();

        public Startup(IConfiguration configuration)
        {
            _container.Options.ResolveUnregisteredConcreteTypes = false;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          //  _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddControllers();

            services.AddSimpleInjector(_container, options =>
            {

                options.AddLogging();

                // AddAspNetCore() wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    .AddControllerActivation();
            });

            // !!!
            AddMassTransitThroughSimpleInjector(services, _container);
            // AddMassTransitThroughCore(services, _container); // works, but without injections
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // !!!!
            ConfigureSimpleInjector(app, _container);

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

        public static IApplicationBuilder ConfigureSimpleInjector(IApplicationBuilder app, Container container)
        {
            // UseSimpleInjector() enables framework services to be injected into
            // application components, resolved by Simple Injector.
            app.UseSimpleInjector(container, options =>
            {
                // Add custom Simple Injector-created middleware to the ASP.NET pipeline.
                // options.UseMiddleware<CustomMiddleware1>(app);
            });

            InitializeContainer(container);

            // Always verify the container
            container.Verify();

            var busControl = container.GetInstance<IBusControl>();
            busControl.Start();

            return app;
        }

        private static void InitializeContainer(Container container)
        {
            // Add application services. For instance:
            container.Register<IClock, SystemClock>(Lifestyle.Scoped);
        }

        public static void AddMassTransitThroughSimpleInjector(IServiceCollection services, Container container)
        {
            container.AddMassTransit(configurator =>
            {
                configurator.AddConsumersFromNamespaceContaining<SendNotificationOnUserCreatedConsumer>();

                configurator.AddBus(() =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri("rabbitmq://myaspcoreapp.rabbitMq"), hostConfigurator =>
                        {
                            hostConfigurator.Username("guest");
                            hostConfigurator.Password("guest");
                        });

                        cfg.ConfigureEndpoints(container);
                    });

                    return bus;
                });
            });
        }

        //public static void AddMassTransitThroughCore(IServiceCollection services, Container container)
        //{
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
        //}
    }
}
