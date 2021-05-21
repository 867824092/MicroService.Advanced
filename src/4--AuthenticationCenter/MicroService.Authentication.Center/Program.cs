namespace MicroService.Authentication.Center {
    using MicroService.Infrastructure.Framework.LogExtensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Program {
        public static void Main(string[] args) {

            Log.Logger = SerilogExtensions.CreateLogger($"{AppDomain.CurrentDomain.BaseDirectory}/log/log.txt");

            Log.Information($"AuthenticationCenterService Start to create");
            try {
             
                var host = CreateHostBuilder(args).Build();

                Log.Information($"AuthenticationCenterService Created Successfully");

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
                });
    }
}
