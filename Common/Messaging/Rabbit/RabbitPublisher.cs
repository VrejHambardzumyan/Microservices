using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Common.Messaging.Rabbit;

public class RabbitPublisher
{
    private readonly RabbitConnection _conn;

    public RabbitPublisher(RabbitConnection conn) => _conn = conn;

    public void Publish<T>(string exchange, string routingKey, T message)
    {
        using var channel = _conn.Connection.CreateModel();
        channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        channel.BasicPublish(exchange, routingKey, props, body);
    }
}