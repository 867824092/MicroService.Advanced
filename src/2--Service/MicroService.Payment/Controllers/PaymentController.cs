using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace MicroService.Payment.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase {
        ILogger<PaymentController> _logger;
        ICapPublisher _capPublisher;
        public PaymentController(ILogger<PaymentController> logger, ICapPublisher capPublisher) {
            _logger = logger;
            _capPublisher = capPublisher;
        }

        [NonAction]
        [DotNetCore.CAP.CapSubscribe("order.create.success")]
        public void Payment(object message) {
            _logger.LogInformation("接收到消息:"+message);
            //await Task.Delay(5000);
            //await _capPublisher.PublishAsync("order.payment.status", "订单支付成功").ConfigureAwait(false);
            _logger.LogInformation($"订单支付成功:{DateTime.Now}");
        }
    }
}
