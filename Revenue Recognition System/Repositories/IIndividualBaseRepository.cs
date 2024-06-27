using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public interface IIndividualBaseRepository : IBaseRepository<Individual>
{
    public Task<Individual?> GetByPesel(string pesel);
}