using System.Text.Json;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.RabbitMQ.Abstract;

public interface IRabbitMqResult
{
    void UseRabbitMessageResult(RabbitMessage message);
}