using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<decimal> GetPriceToPayForSoftware(int softwareContractId)
    {
        return await _dbSet.Where(x => x.ContractId == softwareContractId).SumAsync(x => x.Amount);
    }
}