using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public interface IClientsService
{
    public Task AddClient(IndividualDto client);
    public Task Update(IndividualDto client, int id);
    public Task Delete(int id);
    Task<AbstractClientDTO> Get(int id);
    public Task AddClient(CompanyDto client);
    public Task Update(CompanyDto client, int id);
}