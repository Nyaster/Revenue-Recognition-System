using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Authorize]
[Route("api/revenue")]
public class RevenueController : ControllerBase
{
    private IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }

    [HttpGet]
    public async Task<IActionResult> CalculateRevenue([Optional] int? softwareId, [Optional] string? currency)
    {
        return Ok(await _revenueService.CalculateRevenue(new RevenueRequestModel()
            { ConvertTo = currency, SoftwareId = softwareId }));
    }

    [HttpGet("future")]
    public async Task<IActionResult> CalculateFutureRevenue([Optional] int? softwareId, [Optional] string? currency)
    {
        return Ok(await _revenueService.CalculateFutureRevenue(new RevenueRequestModel()
            { ConvertTo = currency, SoftwareId = softwareId }));
    }
}