using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
{
    public DiscountRepository(AppDbContext context) : base(context)
    {
    }


    public async Task<ICollection<Discount>> GetDiscountsByIds(ICollection<int> ids)
    {
        return await _dbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}