namespace MicroService.Order {
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using MicroService.Infrastructure.Framework;
    using DotNetCore.CAP;
    using DotNetCore.CAP.Dashboard.NodeDiscovery;
    using System;

    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroService.Order", Version = "v1" });
            });
            services.AddGrpc();

            #region CAP
            var rabbitMQOptions = new RabbitMQOptions();
            Configuration.GetSection("RabbitMQ").Bind(rabbitMQOptions);
            services.AddCap(options => {
                options.UseSqlite("Data Source=./cap-event.db");
                options.UseRabbitMQ(config => {
                    config.HostName = rabbitMQOptions.HostName;
                    config.UserName = rabbitMQOptions.UserName;
                    config.Password = rabbitMQOptions.Password;
                    config.Port = rabbitMQOptions.Port;
                    config.VirtualHost = rabbitMQOptions.VirtualHost;
                    config.ExchangeName = rabbitMQOptions.ExchangeName;
                });
                options.UseDashboard();
                //options.UseDiscovery(_ => {
                //    _.DiscoveryServerHostName = "localhost";
                //    _.DiscoveryServerPort = 8500;
                //    _.CurrentNodeHostName = Configuration["Ip"];
                //    _.CurrentNodePort =int.Parse(Configuration["web-port"]);
                //    _.NodeId = Guid.NewGuid().ToString();
                //    _.NodeName = Guid.NewGuid().ToString()+"  "+"Order-Service";
                //});
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService.Order v1"));
            }
          
            app.UseRouting();
            app.UseAuthorization();
            app.UseSerilogRequestLogging(); // <-- Add this line
            app.UseEndpoints(endpoints => {
                endpoints.MapGrpcService<HealthCheckService>();
                endpoints.MapControllers();
            });
        }
    }
}
