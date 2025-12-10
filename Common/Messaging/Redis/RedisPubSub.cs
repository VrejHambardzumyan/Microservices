using StackExchange.Redis;
using System.Text.Json;

namespace Common.Messaging.Redis;

public class RedisPubSub
{
    private readonly ConnectionMultiplexer _mux;
    private readonly ISubscriber _sub;

    public RedisPubSub(string connectionString)
    {
        _mux = ConnectionMultiplexer.Connect(connectionString);
        _sub = _mux.GetSubscriber();
    }

    public Task PublishAsync<T>(string channel, T message)
    {
        var payload = JsonSerializer.Serialize(message);
        return _sub.PublishAsync(channel, payload);
    }

    public void Subscribe<T>(string channel, Action<T> handler)
    {
        _sub.Subscribe(channel, (_, value) =>
        {
            var msg = JsonSerializer.Deserialize<T>(value)!;
            handler(msg);
        });
    }
}