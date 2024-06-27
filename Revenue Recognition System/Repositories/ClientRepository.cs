using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class ClientRepository : BaseRepository<AbstractClient>, IClientsRepository
{
    public ClientRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<AbstractClient?> GetClientById(int id)
    {
        var client = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

        return client;
    }

    public async Task<Individual?> GetByPesel(string pesel)
    {
        return await _context.Individuals.FirstOrDefaultAsync(x => x.Pesel == pesel);
    }

    public async Task<Company?> GetClientByKrs(string krs)
    {
        return await _context.Companies.FirstOrDefaultAsync(x => x.Krs == krs);
    }
    
}