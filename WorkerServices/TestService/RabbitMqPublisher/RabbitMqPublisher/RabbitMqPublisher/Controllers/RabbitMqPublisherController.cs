using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMqPublisher.AsyncDataServices;

namespace RabbitMqPublisher.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RabbitMqPublisherController : ControllerBase
    {
        private readonly IMessageBusClient _client;

        public RabbitMqPublisherController(IMessageBusClient client)
        {
            _client = client;
        }

        [HttpPost(Name = "CreateMessage")]
        public ActionResult CreateMessage([FromHeader] string message)
        {
            _client.SendMessageToSubscriber(message);

            return Ok($"{message} was sent!");
        }
    }
}