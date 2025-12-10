using CatalogService.Domain.Entities;
using CatalogService.Infra.Repositoies;
using CatalogService.Infrastructure.Persistence.Repositories;

namespace CatalogService.Application.Services;

/// <summary>
/// Application service for managing item prices.
/// </summary>
public class PriceService
{
    private readonly PriceRepository _priceRepo;

    public PriceService(PriceRepository priceRepo)
    {
        _priceRepo = priceRepo;
    }

    public async Task<Price?> GetLatestPriceAsync(Guid itemId, string currency)
    {
        return await _priceRepo.GetLatestPriceAsync(itemId, currency);
    }

    public async Task<Price> AddPriceAsync(Guid itemId, decimal amount, string currency)
    {
        var price = new Price
        {
            Id = Guid.NewGuid(),
            ItemId = itemId,
            Amount = amount,
            Currency = currency,
            EffectiveFrom = DateTime.UtcNow
        };
        _priceRepo.Add(price);
        await _priceRepo.SaveAsync();
        return price;
    }
}