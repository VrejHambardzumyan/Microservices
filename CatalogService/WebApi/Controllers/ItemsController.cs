using CatalogService.Infrastructure.Persistence;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.API.Controllers;

/// <summary>
/// Manage catalog items.
/// </summary>
[ApiController]
[Route("api/catalog/items")]
public class ItemsController : ControllerBase
{
    private readonly CatalogDbContext _db;
    public ItemsController(CatalogDbContext db) => _db = db;

    /// <summary>Get active items.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
    public async Task<IActionResult> GetActive()
    {
        var items = await _db.Items.Where(i => i.IsActive).ToListAsync();
        return Ok(items);
    }

    /// <summary>Create a new item.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Item), 201)]
    public async Task<IActionResult> Create(Item item)
    {
        item.Id = Guid.NewGuid();
        _db.Items.Add(item);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetActive), new { id = item.Id }, item);
    }
}