﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Repositories;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[Route("clients")]
[ApiController]
[Authorize]
public class ClientsController : ControllerBase
{
    private IClientsService _clientsService;
    private IClientsRepository _clientsRepository;
    private IContractsService _contractsService;

    public ClientsController(IClientsService clientsService, IClientsRepository clientsRepository,
        IContractsService contractsService)
    {
        _clientsService = clientsService;
        _clientsRepository = clientsRepository;
        _contractsService = contractsService;
    }

    [HttpPost("individual")]
    public async Task<IActionResult> AddINdividualCustomer(IndividualDto customer)
    {
        await _clientsService.AddClient(customer);
        return Ok("Created");
    }

    [HttpPatch("{id:int}/individual")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateIndividualCustomer(IndividualDto customer, int id)
    {
        await _clientsService.Update(customer, id);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteIndividualCustomer(int id)
    {
        await _clientsService.Delete(id);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetIndividualCustomer(int id)
    {
        return Ok(await _clientsService.Get(id));
    }

    [HttpPost("company")]
    public async Task<IActionResult> AddCompanyCustomer(CompanyDto customer)
    {
        await _clientsService.AddClient(customer);
        return Ok("Created");
    }

    [HttpPatch("{id:int}/company")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCompanyCustomer(CompanyDto customer, int id)
    {
        await _clientsService.Update(customer, id);
        return Ok();
    }

    [HttpPost("{id:int}/contracts")]
    public async Task<IActionResult> CreateContract(ContractDto contract, int id)
    {
        await _contractsService.CreateContract(contract, id);
        return Ok();
    }

    [HttpPost("{id:int}/contracts/{contract:int}")]
    public async Task<IActionResult> ProcessPaymend([FromBody] decimal amount, int contract)
    {
        var processPayment = await _contractsService.ProcessPayment(contract, amount);
        return Ok(processPayment);
    }
}