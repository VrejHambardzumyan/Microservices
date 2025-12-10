using CatalogService.Application.Services;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers;

/// <summary>
/// Manage item prices in the catalog.
/// </summary>
[ApiController]
[Route("api/catalog/prices")]
public class PricesController : ControllerBase
{
    private readonly PriceService _priceService;

    public PricesController(PriceService priceService)
    {
        _priceService = priceService;
    }

    /// <summary>
    /// Get the latest price for an item by its ID and currency.
    /// </summary>
    /// <param name="itemId">Item GUID</param>
    /// <param name="currency">Currency code (default USD)</param>
    [HttpGet("{itemId:guid}")]
    [ProducesResponseType(typeof(Price), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetLatest(Guid itemId, [FromQuery] string currency = "USD")
    {
        var price = await _priceService.GetLatestPriceAsync(itemId, currency);
        if (price == null) return NotFound(new { message = "Price not found" });
        return Ok(price);
    }

    /// <summary>
    /// Add a new price for an item.
    /// </summary>
    /// <param name="itemId">Item GUID</param>
    /// <param name="amount">Price amount</param>
    /// <param name="currency">Currency code</param>
    [HttpPost("{itemId:guid}")]
    [ProducesResponseType(typeof(Price), 201)]
    public async Task<IActionResult> Add(Guid itemId, [FromQuery] decimal amount, [FromQuery] string currency = "USD")
    {
        var price = await _priceService.AddPriceAsync(itemId, amount, currency);
        return CreatedAtAction(nameof(GetLatest), new { itemId = itemId, currency = currency }, price);
    }
}