using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class ItemRepository
{
    private readonly CatalogService.Infrastructure.Persistence.CatalogDbContext _db;
    public ItemRepository(CatalogService.Infrastructure.Persistence.CatalogDbContext db) => _db = db;

    public Task<Item?> GetBySkuAsync(string sku) =>
        _db.Items.Include(i => i.Prices).FirstOrDefaultAsync(i => i.Sku == sku);

    public void Add(Item item) => _db.Items.Add(item);
    public Task SaveAsync() => _db.SaveChangesAsync();
}