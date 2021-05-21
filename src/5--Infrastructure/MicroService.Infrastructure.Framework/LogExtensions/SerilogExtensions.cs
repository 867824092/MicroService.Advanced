#region 
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion
namespace MicroService.Infrastructure.Framework.LogExtensions {
    public  class SerilogExtensions {
        public static Serilog.Core.Logger CreateLogger(string file) {
            return new LoggerConfiguration()
                 .MinimumLevel.Information()
                 .MinimumLevel.Override("Microsoft",Serilog.Events.LogEventLevel.Information)
                 .MinimumLevel.Override("System",Serilog.Events.LogEventLevel.Information)
                 .MinimumLevel.Override("Microsoft.AspNetCore",Serilog.Events.LogEventLevel.Warning)
                 //.Enrich.WithProperty("ApplicationContext", "MicroService.Order")
                 .Enrich.FromLogContext()
                 .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}.{Method}] {Message:lj}{NewLine}{Exception}")
                 .WriteTo.File(file,
                  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                  rollingInterval: RollingInterval.Day,
                  rollOnFileSizeLimit: true,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] [{SourceContext}.{Method}] {Message:lj}{NewLine}{Exception}")
                 .Filter.ByExcluding(Matching.WithProperty("SourceContext", "Microsoft.AspNetCore.Hosting.Diagnostics"))
                 //.ReadFrom.Configuration(configuration)
                 .CreateLogger();
        }

        public static LoggerConfiguration CreateLoggerConfiguration(string file) {
            return new LoggerConfiguration()
                 .MinimumLevel.Information()
                 .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                 .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Information)
                 //.Enrich.WithProperty("ApplicationContext", "MicroService.Order")
                 .Enrich.FromLogContext()
                 .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}.{Method}] {Message:lj}{NewLine}{Exception}")
                 .WriteTo.File(file,
                  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                  rollingInterval: RollingInterval.Day,
                  rollOnFileSizeLimit: true,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] [{SourceContext}.{Method}] {Message:lj}{NewLine}{Exception}")
                 //.ReadFrom.Configuration(configuration)
                 ;
        }

    }
}
