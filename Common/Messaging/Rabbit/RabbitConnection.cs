using RabbitMQ.Client;

namespace Common.Messaging.Rabbit;

public class RabbitConnection
{
    public IConnection Connection { get; }
    public RabbitConnection(IConfiguration config)
    {
        var factory = new ConnectionFactory
        {
            HostName = config["Rabbit:Host"] ?? "localhost",
            UserName = config["Rabbit:User"] ?? "guest",
            Password = config["Rabbit:Password"] ?? "guest",
            DispatchConsumersAsync = true
        };
        Connection = factory.CreateConnection();
    }
}