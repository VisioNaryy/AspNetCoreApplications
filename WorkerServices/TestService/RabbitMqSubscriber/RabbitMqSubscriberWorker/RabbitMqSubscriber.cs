using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMqSubscriberWorker
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqSubscriber> _logger;

        private IConnection _connection;
        private IModel _channel;
        private string _consumerTag;
        private string _queueName;

        public RabbitMqSubscriber(IConfiguration configuration, ILogger<RabbitMqSubscriber> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionFactory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };

            _connection = connectionFactory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            //_queueName = _channel.QueueDeclare().QueueName;
            _queueName = "videoreceived.queue";
            _channel.QueueBind(queue: _queueName,
                exchange: "trigger",
                routingKey: "");
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            _logger.LogInformation("Waiting for Messages...");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                _logger.LogInformation($"[x] {message}");

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _consumerTag = _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            _logger.LogInformation(" Press [enter] to exit.");

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.BasicCancel(_consumerTag);
            _channel.Close();
            _connection.Close();

            return StopAsync(cancellationToken);
        }
    }
}