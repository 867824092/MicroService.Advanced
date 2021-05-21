namespace MicroService.Infrastructure.Framework {
    using Grpc.Core;
    using Grpc.Health.V1;
    using Grpc.HealthCheck;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Grpc 检查检查
    /// </summary>
    public class HealthCheckService : HealthServiceImpl {
        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context) {
            return Task.FromResult(new HealthCheckResponse() { Status = HealthCheckResponse.Types.ServingStatus.Serving });
        }

        public override async Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context) {
            await responseStream.WriteAsync(new HealthCheckResponse() { Status = HealthCheckResponse.Types.ServingStatus.Serving });
        }
    }
}
