using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Persistence.Repositories;

namespace CatalogService.Application.Services;

/// <summary>
/// Application service for managing catalog items.
/// </summary>
public class ItemService
{
    private readonly ItemRepository _itemRepo;

    public ItemService(ItemRepository itemRepo)
    {
        _itemRepo = itemRepo;
    }

    public async Task<Item?> GetBySkuAsync(string sku) =>
        await _itemRepo.GetBySkuAsync(sku);

    public async Task<Item> CreateAsync(string sku, string name)
    {
        var item = new Item { Id = Guid.NewGuid(), Sku = sku, Name = name };
        _itemRepo.Add(item);
        await _itemRepo.SaveAsync();
        return item;
    }
}