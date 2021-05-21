namespace MicroService.Order.Controllers {
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DotNetCore.CAP;

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        ILogger<OrderController> _logger;
        ICapPublisher _capPublisher;
        public OrderController(ILogger<OrderController> logger,ICapPublisher capPublisher) {
            _logger = logger;
            _capPublisher = capPublisher;
        }

        [HttpGet]
        public string Get([FromServices]IConfiguration configuration) {
            return DateTime.Now.ToString("端口号:【"+configuration["web-port"]+ "】yyyy-MM-dd HH:mm:ss");
        }
        [HttpGet("create")]
        public async Task CreateOrder() {
            await Task.Delay(2000);
            await _capPublisher.PublishAsync("order.create.success", $"创建订单成功:{DateTime.Now}", "order.payment.status").ConfigureAwait(false);
            _logger.LogInformation($"创建订单成功:{DateTime.Now}");
        }

        /*
         * 订单支付完成回调
         */
        [NonAction]
        [DotNetCore.CAP.CapSubscribe("order.payment.status")]
        public void UpdateOrderStatus() {
            _logger.LogInformation("支付后的回调");
        }
    }
}
