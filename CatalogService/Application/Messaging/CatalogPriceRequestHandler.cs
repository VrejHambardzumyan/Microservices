using Common.Messaging.DTO;
using Common.Messaging.Rabbit;
using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.Messaging;

public class CatalogPriceRequestHandler
{
    private readonly CatalogDbContext _db;
    private readonly RabbitPublisher _publisher;

    public CatalogPriceRequestHandler(CatalogDbContext db, RabbitPublisher publisher)
    {
        _db = db;
        _publisher = publisher;
    }

    public async Task HandleAsync(MessageEnvelope<GetItemPriceRequestDto> envelope)
    {
        var sku = envelope.Payload.ItemSku;
        var currency = envelope.Payload.Currency;

        var item = await _db.Items.Include(i => i.Prices)
            .FirstOrDefaultAsync(i => i.Sku == sku && i.IsActive);

        var price = item?.Prices
            .Where(p => p.Currency == currency && (p.EffectiveTo == null || p.EffectiveTo > DateTime.UtcNow))
            .OrderByDescending(p => p.EffectiveFrom)
            .FirstOrDefault();

        var response = new MessageEnvelope<GetItemPriceResponseDto>
        {
            CorrelationId = envelope.CorrelationId, 
            Payload = new GetItemPriceResponseDto
            {
                ItemSku = sku,
                Currency = currency,
                Found = price != null,
                Amount = price?.Amount ?? 0m,
                Error = item == null ? "Item not found or inactive" : (price == null ? "Price not found" : null)
            }
        };

        _publisher.Publish(RabbitExchangeNames.EnrollmentExchange, "price.response", response);
    }
}