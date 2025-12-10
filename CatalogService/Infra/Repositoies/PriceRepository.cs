using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for accessing Price entities.
/// </summary>
public class PriceRepository
{
    private readonly CatalogDbContext _db;

    public PriceRepository(CatalogDbContext db) => _db = db;

    public async Task<Price?> GetLatestPriceAsync(Guid itemId, string currency)
    {
        return await _db.Prices
            .Where(p => p.ItemId == itemId && p.Currency == currency &&
                        (p.EffectiveTo == null || p.EffectiveTo > DateTime.UtcNow))
            .OrderByDescending(p => p.EffectiveFrom)
            .FirstOrDefaultAsync();
    }

    public void Add(Price price) => _db.Prices.Add(price);

    public Task SaveAsync() => _db.SaveChangesAsync();
}