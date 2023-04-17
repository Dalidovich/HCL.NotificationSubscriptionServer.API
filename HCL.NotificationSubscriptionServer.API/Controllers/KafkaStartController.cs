using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HCL.NotificationSubscriptionServer.API.Controllers
{
    public class KafkaStartController : ControllerBase
    {
        private readonly IKafkaConsumerService _kafkaConsumerService;

        public KafkaStartController(IKafkaConsumerService kafkaConsumerService)
        {
            _kafkaConsumerService = kafkaConsumerService;
        }

        [HttpGet("v1/listen")]
        public async Task<IActionResult> Get()
        {
            await _kafkaConsumerService.Listen();

            return Ok();
        }
    }
}
