namespace RabbitMqPublisher.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void SendMessageToSubscriber(string message);
    }
}