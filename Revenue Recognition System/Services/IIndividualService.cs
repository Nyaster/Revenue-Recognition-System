using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Services;

public interface IIndividualService
{
    public Task AddClient(IndividualDto client);
    public Task Update(IndividualDto client, int id);
    public Task Delete(int id);
    Task<IndividualDto> Get(int id);
}