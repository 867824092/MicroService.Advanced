namespace MicroService.GateWay {
    using MicroService.Infrastructure.Framework.LogExtensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Program {
        public static void Main(string[] args) {
            Log.Logger = SerilogExtensions.CreateLoggerConfiguration($"{AppDomain.CurrentDomain.BaseDirectory}/log/log.txt")
                                          .Filter.ByExcluding(Matching.WithProperty("SourceContext", "Ocelot.Configuration.Repository.FileConfigurationPoller"))
                                          .CreateLogger();

            Log.Information($"GateWay Start to create");
            try {

                var host = CreateHostBuilder(args).Build();

                Log.Information($"GateWay Created Successfully");

                host.Run();
            }
            catch (Exception ex) {
                Log.Fatal(ex.Message);
                return;
            }
            finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => {
                   webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>{
                   config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
                });
    }
}
