using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class SoftwareContractRepostory : BaseRepository<SoftwareContract>, ISoftwareContractRepository
{
    public SoftwareContractRepostory(AppDbContext context) : base(context)
    {
    }

    public async Task<decimal> GetPayedAllMoney()
    {
        return await _dbSet.Where(x => x.IsPaid == true).SumAsync(x => x.Price);
    }

    public async Task<decimal> GetPayedAllMoney(int? softwareId)
    {
        return await _dbSet.Where(x => x.IsPaid == true && x.SoftwareId == softwareId).SumAsync(x => x.Price);
    }

    public async Task<decimal> GetAllMoney()
    {
        return await _dbSet.SumAsync(x => x.Price);
    }

    public async Task<decimal> GetAllMoney(int? softwareId)
    {
        return await _dbSet.Where(x => x.SoftwareId == softwareId).SumAsync(x => x.Price);
    }
}