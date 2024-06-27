using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[Route("Individual")]
[ApiController]
[Authorize]
public class IndividualController : ControllerBase
{
    private IIndividualService _individualService;

    public IndividualController(IIndividualService individualService)
    {
        _individualService = individualService;
    }

    [HttpPost]
    public async Task<IActionResult> AddCompanyCustomer(IndividualDto customer)
    {
        await _individualService.AddClient(customer);
        return Ok("Created");
    }

    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCustomer(IndividualDto customer, int id)
    {
        await _individualService.Update(customer, id);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _individualService.Delete(id);
        return Ok();
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        
        return Ok(await _individualService.Get(id));
    }
}