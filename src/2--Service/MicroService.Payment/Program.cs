using MicroService.Infrastructure.Framework.LogExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NConsul.AspNetCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MicroService.Payment {
    public class Program {
            /*
        *  ��������
        *  Api�˿ڷ�Χ��10510~10519
        *  Grpc�˿ڷ�Χ��10520~10529
        *  ������� --web-port  --grpc-port
        *  dotnet .\MicroService.Payment.dll --web-port=10510 --grpc-port=10520
        *  dotnet .\MicroService.Payment.dll --web-port=10511 --grpc-port=10521
        */
        readonly static string web_port = "web-port";
        readonly static string grpc_port ="grpc-port";
        public static void Main(string[] args) {
            Log.Logger = SerilogExtensions.CreateLogger($"{AppDomain.CurrentDomain.BaseDirectory}/log/log.txt");

            Log.Information($"PaymentService Start to Create");

            if (args.Length == 0) {
                Log.Error("agrs is must  web-port   grpc-port");
                return;
            }
            try {

                var host = CreateHostBuilder(args).Build();

                Log.Information($"PaymentService Created Successfully");

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
         .UseSerilog() // <-- Add this line
         .ConfigureAppConfiguration((context, builder) => { //���ݻ���������Ӳ�ͬ���ļ�
                    if (!context.HostingEnvironment.IsProduction()) {
                 builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true);
             }
             builder.AddCommandLine(args);//�����������
                })
         .ConfigureWebHostDefaults(webBuilder => {
             webBuilder.ConfigureKestrel((context, options) => { // API GRPC������ַ
                 var configuration = context.Configuration;
                 string ip = configuration["Ip"];
                 options.Listen(IPAddress.Parse(ip), int.Parse(configuration[web_port]), c => c.Protocols = HttpProtocols.Http1);
                 options.Listen(IPAddress.Parse(ip), int.Parse(configuration[grpc_port]), c => c.Protocols = HttpProtocols.Http2);
             });
             webBuilder.UseStartup<Startup>();
         })
         .ConfigureServices((context, services) => {
             var configuration = context.Configuration;
             var section = configuration.GetSection("Consul");
             var ip = configuration["Ip"];

             services.AddConsul(section.GetValue<string>("ClusterAddress"))
                     .AddGRPCHealthCheck(ip + ":" + configuration[grpc_port], interval: 60)
                     .RegisterService(
                         section.GetValue<string>("RegisterService"),
                         ip,
                         int.Parse(configuration[web_port]),
                         null);
         });
    }
}
