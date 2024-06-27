using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public interface IClientsRepository : IBaseRepository<AbstractClient>
{
    public Task<AbstractClient?> GetClientById(int id);
    public Task<Individual?> GetByPesel(string pesel);
    public Task<Company?> GetClientByKrs(string id);
}