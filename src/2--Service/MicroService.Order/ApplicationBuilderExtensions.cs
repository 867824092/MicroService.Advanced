namespace MicroService.Order {
    using Microsoft.AspNetCore.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Consul;

    /*
     * 注册Consul
     * 
     */
    public static class ApplicationBuilderExtensions {

        public static async Task UseConsul(this IApplicationBuilder applicationBuilder) {
            using ConsulClient client = new(config=>{
                config.Address = new Uri("http://localhost:8500");
                config.Datacenter = "dc1";
            });
            await client.Agent.ServiceRegister(new AgentServiceRegistration {
                  Address = "localhost",
                  Check = new AgentServiceCheck {
                       Interval = TimeSpan.FromSeconds(10),
                       GRPC = "localhost:10005",
                       Name= "grpc-check",
                       Status = HealthStatus.Passing,
                       DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20),
                  },
                  Port = 10004,
                  ID = "OrderServer-1",
                  Name = "OrderService"
                });
        }
    }
}
