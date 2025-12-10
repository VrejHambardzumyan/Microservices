using Microsoft.AspNetCore.Mvc;
using EnrollmentService.Application.Services;

namespace EnrollmentService.API.Controllers;

/// <summary>
/// Handle enrollments lifecycle.
/// </summary>
[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly EnrollmentService.Application.Services.EnrollmentService _service;
    public EnrollmentsController(EnrollmentService.Application.Services.EnrollmentService service) => _service = service;

    /// <summary>Create an enrollment for a user and item SKU.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), 202)]
    public async Task<IActionResult> Create([FromQuery] Guid userId, [FromQuery] string sku)
    {
        var id = await _service.CreateEnrollmentAsync(userId, sku);
        return Accepted(new { enrollmentId = id, status = "Pending" });
    }
}