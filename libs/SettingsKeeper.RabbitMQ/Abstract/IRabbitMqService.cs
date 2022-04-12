namespace SettingsKeeper.RabbitMQ.Abstract;

public interface IRabbitMqService
{
    void CreateNewRabbitQueue(string queueName);

    void SendMessage<T>(string queueName, T message) where T: class;
}