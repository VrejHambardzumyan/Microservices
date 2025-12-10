using Common.Messaging.DTO;
using Common.Messaging.Rabbit;

namespace EnrollmentService.Application.Messaging;

/// <summary>
/// Publishes a request to CatalogService to get item price.
/// </summary>
public class EnrollmentPriceRequester
{
    private readonly RabbitPublisher _publisher;

    public EnrollmentPriceRequester(RabbitPublisher publisher) => _publisher = publisher;

    /// <summary>
    /// Sends a GetItemPriceRequest message and returns the correlation ID.
    /// </summary>
    public Guid RequestItemPrice(string sku, string currency = "USD")
    {
        var envelope = new MessageEnvelope<GetItemPriceRequestDto>
        {
            CorrelationId = Guid.NewGuid(),
            Payload = new GetItemPriceRequestDto { ItemSku = sku, Currency = currency }
        };

        _publisher.Publish(RabbitExchangeNames.CatalogExchange, "price.request", envelope);
        return envelope.CorrelationId;
    }
}