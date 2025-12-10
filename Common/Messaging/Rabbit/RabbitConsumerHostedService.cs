using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Common.Messaging.Rabbit;
using Common.Messaging.DTO;

public class RabbitConsumerHostedService : BackgroundService
{
    private readonly RabbitConnection _conn;
    private readonly IServiceProvider _sp;

    public RabbitConsumerHostedService(RabbitConnection conn, IServiceProvider sp)
    {
        _conn = conn;
        _sp = sp;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _conn.Connection.CreateModel();
        channel.ExchangeDeclare(RabbitExchangeNames.CatalogExchange, ExchangeType.Direct, true);
        channel.ExchangeDeclare(RabbitExchangeNames.EnrollmentExchange, ExchangeType.Direct, true);

        // Declare queues
        channel.QueueDeclare(RabbitExchangeNames.PriceRequestsQueue, durable: true, exclusive: false, autoDelete: false);
        channel.QueueDeclare(RabbitExchangeNames.PriceResponsesQueue, durable: true, exclusive: false, autoDelete: false);

        // Bind queues
        channel.QueueBind(RabbitExchangeNames.PriceRequestsQueue, RabbitExchangeNames.CatalogExchange, "price.request");
        channel.QueueBind(RabbitExchangeNames.PriceResponsesQueue, RabbitExchangeNames.EnrollmentExchange, "price.response");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (_, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var type = ea.BasicProperties?.Type ?? string.Empty;

            using var scope = _sp.CreateScope();

            // Dispatch by routing key
            if (ea.RoutingKey == "price.request")
            {
                var envelope = JsonSerializer.Deserialize<MessageEnvelope<GetItemPriceRequestDto>>(json)!;
                var handler = scope.ServiceProvider.GetRequiredService<CatalogService.Application.Messaging.CatalogPriceRequestHandler>();
                await handler.HandleAsync(envelope);
            }
            else if (ea.RoutingKey == "price.response")
            {
                var envelope = JsonSerializer.Deserialize<MessageEnvelope<GetItemPriceResponseDto>>(json)!;
                var handler = scope.ServiceProvider.GetRequiredService<EnrollmentService.Application.Messaging.EnrollmentPriceResponseHandler>();
                await handler.HandleAsync(envelope);
            }

            channel.BasicAck(ea.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(RabbitExchangeNames.PriceRequestsQueue, autoAck: false, consumer);
        channel.BasicConsume(RabbitExchangeNames.PriceResponsesQueue, autoAck: false, consumer);

        return Task.CompletedTask;
    }
}